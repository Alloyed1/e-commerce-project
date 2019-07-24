using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using e_commerce_project.Models;
using e_commerce_project.ViewModel.Account;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace e_commerce_project.Repositories
{
	public interface IShopRepository
	{
		Task<Item> GetItemInfo(int itemId);
		Task<ItemShopViewModel> GetItem(int itemId);
		Task<bool> AddToFavorit(string userEmail, int itemId);
		Task<bool> AddToBaskets(string userEmail, int itemId);
		Task<List<ItemShopViewModel>> GetFavoriteList(string userEmail);
		Task<List<ItemShopViewModel>> GetBasketList(string userEmail);
		Task DeleteFromFavorite (string userEmail, int itemId);
		Task DeleteFromBasket(string userEmail, int itemId);
		Task<string> GetUserId(string userEmail);
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
		public async Task DeleteFromBasket(string userEmail, int itemId)
		{
			using (IDbConnection db = new SqlConnection(connectionString))
			{

				string userId = await GetUserId(userEmail);
				var basketResult = await db.QueryAsync<Basket>("SELECT * FROM Baskets WHERE UserId = @userId", new { userId });
				Basket basket = basketResult.FirstOrDefault();
				List<Item> items = JsonConvert.DeserializeObject<List<Item>>(basket.BasketItemsList);
				items.RemoveAll(f => f.Id == itemId);
				string newBasketList = JsonConvert.SerializeObject(items);

				await db.ExecuteAsync("UPDATE Baskets SET BasketItemsList = @newBasketList", new { newBasketList });
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
		public async Task<List<ItemShopViewModel>> GetBasketList(string userEmail)
		{
			using (IDbConnection db = new SqlConnection(connectionString))
			{
				string userId = await GetUserId(userEmail);
				var basketResult = await db.QueryAsync<Basket>("SELECT * FROM Baskets WHERE UserId = @userId", new { userId });
				Basket basket = basketResult.FirstOrDefault();
				List<Item> items = JsonConvert.DeserializeObject<List<Item>>(basket.BasketItemsList);
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
		public async Task<bool> AddToBaskets(string userEmail, int itemId)
		{
			using (IDbConnection db = new SqlConnection(connectionString))
			{
				var userIdResult = await db.QueryAsync<string>("SELECT Id FROM AspNetUsers WHERE Email = @userEmail", new { userEmail });
				string userId = userIdResult.FirstOrDefault();


				var itemsBasketListResult =  await db.QueryAsync<string>("SELECT BasketItemsList FROM Baskets WHERE UserId = @userId", new { userId });
				string itemsBasketList;
				List<Item> items = new List<Item>();
				if (itemsBasketListResult.Count() == 0)
					await db.ExecuteAsync("INSERT INTO Baskets (UserId) VALUES(@userId)", new { userId });
				else
				{
					itemsBasketList = itemsBasketListResult.FirstOrDefault();
					if(itemsBasketList != null)
						items = JsonConvert.DeserializeObject<List<Item>>(itemsBasketList);
					
				}


				var itemResult = await db.QueryAsync<Item>("SELECT * FROM Items WHERE Id = @itemId", new { itemId });
				Item item = itemResult.FirstOrDefault();

				Item searchResult = items.Where(i => i.Id == item.Id).FirstOrDefault();
				 
				if (searchResult == null)
					items.Add(item);
				else
					return false;


				itemsBasketList = JsonConvert.SerializeObject(items);
				await db.ExecuteAsync("UPDATE Baskets SET BasketItemsList = @itemsBasketList", new { itemsBasketList });
				return true;
			}
		}
		public async Task<ItemShopViewModel> GetItem(int itemId)
		{
			using (IDbConnection dbDapper = new SqlConnection(connectionString))
			{
				var result = await dbDapper.QueryAsync<Item>("SELECT * FROM Items WHERE Id = @itemId", new { itemId });
				Item item = result.FirstOrDefault();


				ItemShopViewModel itemShop = new ItemShopViewModel();

				itemShop.Id = item.Id;
				itemShop.Name = item.Name;
				itemShop.About = item.About;
				itemShop.Price = item.Price;
				itemShop.Discount = item.Discount;
				itemShop.Brand = item.Brand;
				itemShop.Category = item.Category;
				itemShop.IsDelete = item.IsDelete;
				itemShop.IsHide = item.IsHide;
				itemShop.SizesDictionary = JsonConvert.DeserializeObject<Dictionary<int, int>>(item.SizesDictionary);
				itemShop.AdvantagesArray = JsonConvert.DeserializeObject<string[]>(item.AdvantagesArray);
				itemShop.ItemImagesLinks = JsonConvert.DeserializeObject<string[]>(item.ItemImagesLinks);

				return itemShop;
			}
		}
	}
}
