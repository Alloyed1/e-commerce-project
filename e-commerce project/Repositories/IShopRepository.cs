using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using e_commerce_project.Models;
using e_commerce_project.ViewModel.Account;
using e_commerce_project.ViewModel.Shop;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace e_commerce_project.Repositories
{
	public interface IShopRepository
	{
		Task<Item> GetItemInfo(int itemId);
		Task<ItemShopViewModel> GetItem(int itemId);
		Task<bool> AddToFavorit(string userEmail, int itemId);
		Task<bool> AddToBaskets(string userEmail, int itemId, int size, int count);
		Task<List<ItemShopViewModel>> GetFavoriteList(string userEmail);
		Task<List<BasketViewModel>> GetBasketList(string userEmail);
		Task DeleteFromFavorite (string userEmail, int itemId);
		Task DeleteFromBasket(string userEmail, int itemId, int itemSize);
		Task<string> GetUserId(string userEmail);

		Task AddOrder(string userEmail, List<BasketViewModel> items, int sum);
	}
	public class ShopRepository: IShopRepository
	{
		string connectionString;
		public ShopRepository(IConfiguration configuration)
		{
			connectionString = configuration.GetConnectionString("DefaultConnection");
		}
		public async Task<string> GetUserId(string userEmail){
			using(IDbConnection db = new SqlConnection(connectionString)){
				var userIdResult = await db.QueryAsync<string>("SELECT Id FROM AspNetUsers WHERE Email = @userEmail", new { userEmail });
				return userIdResult.FirstOrDefault();
			}
		}
		public async Task<Item> GetItemInfo(int itemId)
		{
			using(IDbConnection db = new SqlConnection(connectionString))
			{
				var query = "SELECT * FROM Items WHERE Id = @itemId";
				var result = await db.QueryAsync<Item>(query, new { itemId });
				return result.FirstOrDefault();
			}
		}
		public async Task DeleteFromFavorite(string userEmail, int itemId){
			using(IDbConnection db = new SqlConnection(connectionString)){

				string userId = await GetUserId(userEmail);
				var favoriteResult = await db.QueryAsync<Favorite>("SELECT * FROM Favorites WHERE UserId = @userId", new { userId } );
				Favorite favorite = favoriteResult.FirstOrDefault();
				List<Item> items = JsonConvert.DeserializeObject<List<Item>>(favorite.FavoriteItemsList);
				items.RemoveAll(f => f.Id == itemId);
				string newFavoriteList = JsonConvert.SerializeObject(items);

				await db.ExecuteAsync("UPDATE Favorites SET FavoriteItemsList = @newFavoriteList", new { newFavoriteList });
			}
		}
		public async Task DeleteFromBasket(string userEmail, int itemId, int itemSize)
		{
			using (var db = new SqlConnection(connectionString))
			{
				string userId = await GetUserId(userEmail);
				var query = @"DELETE FROM Baskets WHERE UserId = @userId AND Size = @itemSize";
				await db.ExecuteAsync(query, new { userId, itemSize });
			}
		}

		public async Task AddOrder(string userEmail, List<BasketViewModel> items, int sum)
		{
			using(var db = new SqlConnection(connectionString))
			{
				string userId = await GetUserId(userEmail);
				DateTime date = DateTime.Now;

				var query = @"INSERT INTO Orders (Date, Sum, UserId) VALUES (@date, @sum, @userId); SELECT CAST(SCOPE_IDENTITY() as int)";
				int orderId = await db.QueryFirstOrDefaultAsync<int>(query, new { date, sum, userId });
				
				foreach(var item in items)
				{
					int id = item.Id;

					string sizes = await db.QueryFirstOrDefaultAsync<string>("SELECT SizesDictionary FROM Items WHERE Id = @id", new { id });
					Dictionary<int, int> sizesDictionary = JsonConvert.DeserializeObject<Dictionary<int, int>>(sizes);
					sizesDictionary[item.Size] = sizesDictionary[item.Size] - item.Count;
					sizes = JsonConvert.SerializeObject(sizesDictionary);

					await db.ExecuteAsync(@"UPDATE Items SET CountOfPurchases = CountOfPurchases + 1, SizesDictionary = @sizes WHERE Id = @id", new { id, sizes });



					await db.ExecuteAsync(@"INSERT INTO ItemsOrders (OrderId, ItemId, Size, Count) VALUES (@orderId, @Id, @Size, @Count)", new { orderId, item.Id, item.Size, item.Count });


					
				}
				await db.ExecuteAsync("DELETE FROM Baskets WHERE UserId = @userId", new { userId });
			}
		}
		public async Task<List<ItemShopViewModel>> GetFavoriteList(string userEmail)
		{
			using(IDbConnection db = new SqlConnection(connectionString))
			{
				string userId = await GetUserId(userEmail);
				var favoriteResult = await db.QueryAsync<Favorite>("SELECT * FROM Favorites WHERE UserId = @userId", new { userId });
				Favorite favorite = favoriteResult.FirstOrDefault();
				List<Item> items = JsonConvert.DeserializeObject<List<Item>>(favorite.FavoriteItemsList);
				List<ItemShopViewModel> itemShopList = new List<ItemShopViewModel>();
				foreach (var item in items)
				{
					ItemShopViewModel itemShop = new ItemShopViewModel();
					itemShop.Id = item.Id;
					itemShop.Name = item.Name;
					itemShop.About = item.About;
					itemShop.Price = item.Price;
					itemShop.Discount = item.Discount;
					itemShop.Category = item.Category;
					
					itemShop.IsDelete = item.IsDelete;
					itemShop.IsHide = item.IsHide;
					itemShop.SizesDictionary = JsonConvert.DeserializeObject<Dictionary<int, int>>(item.SizesDictionary);
					itemShop.ItemImagesLinks = JsonConvert.DeserializeObject<string[]>(item.ItemImagesLinks);

					itemShopList.Add(itemShop);
				}
				return itemShopList;

			}
		}
		public async Task<List<BasketViewModel>> GetBasketList(string userEmail)
		{
			using (IDbConnection db = new SqlConnection(connectionString))
			{
				string userId = await GetUserId(userEmail);
				IEnumerable<Basket> baskets = await db.QueryAsync<Basket>("SELECT * FROM Baskets WHERE UserId = @userId", new { userId });
				var items = (await db.QueryAsync<Item>("SELECT Name, About, Price, Discount, Id, ItemImagesLinks FROM Items")).ToList();
				var basketViewModels = new List<BasketViewModel>();
				foreach(var basket in baskets)
				{
					BasketViewModel basketView = new BasketViewModel();
					Item item = items.FirstOrDefault(w => w.Id == basket.ItemId);
					basketView.Name = item.Name;
					basketView.About = item.About;
					basketView.Price = item.Price;
					basketView.Size = basket.Size;
					basketView.Id = item.Id;
					basketView.Discount = (int)item.Discount;
					basketView.ImageUrl = JsonConvert.DeserializeObject<List<string>>(item.ItemImagesLinks)[0];
					basketViewModels.Add(basketView);
				}
				


				return basketViewModels;
			}
		}
		public async Task<bool> AddToFavorit(string userEmail, int itemId)
		{
			using(IDbConnection db = new SqlConnection(connectionString))
			{
				var userIdResult = await db.QueryAsync<string>("SELECT Id FROM AspNetUsers WHERE Email = @userEmail", new { userEmail });
				string userId = userIdResult.FirstOrDefault();


				var itemsFavoriteListResult = await db.QueryAsync<string>("SELECT FavoriteItemsList FROM Favorites WHERE UserId = @userId", new { userId });
				string itemsFavoriteList;
				List<Item> items = new List<Item>();

				if (itemsFavoriteListResult.Count() == 0)
					await db.ExecuteAsync("INSERT INTO Favorites (UserId) VALUES(@userId)", new { userId });
				else
				{
					itemsFavoriteList = itemsFavoriteListResult.FirstOrDefault();
					if(itemsFavoriteList != null)
						items = JsonConvert.DeserializeObject<List<Item>>(itemsFavoriteList);
				}


				var itemResult = await db.QueryAsync<Item>("SELECT * FROM Items WHERE Id = @itemId", new { itemId });
				Item item = itemResult.FirstOrDefault();

				var searchResult = items.Where(i => i.Id == item.Id).FirstOrDefault();

				if (searchResult == null)
				 items.Add(item);
				else
					return false;


				itemsFavoriteList = JsonConvert.SerializeObject(items);
				await db.ExecuteAsync("UPDATE Favorites SET FavoriteItemsList = @itemsFavoriteList", new { itemsFavoriteList });
				return true;

			}
		}
		public async Task<bool> AddToBaskets(string userEmail, int itemId, int size, int count)
		{
			using (IDbConnection db = new SqlConnection(connectionString))
			{
				string userId = await GetUserId(userEmail);

				IEnumerable<Basket> userBasket = await db.QueryAsync<Basket>("SELECT Count, Size, ItemId FROM Baskets WHERE UserId = @userId", new { userId });

				Basket searchBasket = userBasket.FirstOrDefault(b => b.ItemId == itemId && b.Size == size);
				if(searchBasket != null)
					return false;
				string query = "INSERT INTO Baskets (UserId, ItemId, Count, Size ) VALUES(@userId, @itemId, @count, @size)";
				await db.ExecuteAsync(query, new { userId, itemId, count, size });
				return true;
			}
		}
		public async Task<ItemShopViewModel> GetItem(int itemId)
		{
			using (IDbConnection dbDapper = new SqlConnection(connectionString))
			{
				var result = await dbDapper.QueryAsync<Item>("SELECT * FROM Items WHERE Id = @itemId", new { itemId });
				Item item = result.FirstOrDefault();


				ItemShopViewModel itemShop = new ItemShopViewModel
				{
					Id = item.Id,
					Name = item.Name,
					About = item.About,
					Price = item.Price,
					Discount = item.Discount,
					Category = item.Category,
					IsDelete = item.IsDelete,
					IsHide = item.IsHide,
					CountOfPurchases = item.CountOfPurchases,
					SizesDictionary = JsonConvert.DeserializeObject<Dictionary<int, int>>(item.SizesDictionary),
					AdvantagesArray = JsonConvert.DeserializeObject<string[]>(item.AdvantagesArray),
					ItemImagesLinks = JsonConvert.DeserializeObject<string[]>(item.ItemImagesLinks)
				};

				return itemShop;
			}
		}
	}
}
