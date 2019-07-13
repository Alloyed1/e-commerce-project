using e_commerce_project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using e_commerce_project.ViewModel.Account;
using Newtonsoft.Json;

namespace e_commerce_project.Repositories
{

	public interface IAdminRepository
	{
		Task AddItem(Item item);
		Task<List<ItemShopViewModel>> GetAllItems();
		Task HideItem(int itemId);
		Task HideItemOff(int itemId);
	}
	public class AdminRepository: IAdminRepository
	{
		string connectionString;
		public AdminRepository(IConfiguration configuration)
		{
			connectionString = configuration.GetConnectionString("DefaultConnection");
		}

		public async Task AddItem(Item item)
		{
			using(IDbConnection dbDapper = new SqlConnection(connectionString))
			{
				var query = "INSERT INTO Items (Name, Price, About,CountOfPurchases, AddItemTime, SizesDictionary, AdvantagesArray, Discount)" +
					" VALUES (@Name, @Price, @About,@CountOfPurchases, @AddItemTime, @SizesDictionary, @AdvantagesArray, @Discount);" +
				"SELECT CAST(SCOPE_IDENTITY() as int)";

				await dbDapper.ExecuteAsync(query, new { item.Name, item.Price, item.About, item.CountOfPurchases, item.AddItemTime, item.SizesDictionary, item.AdvantagesArray, item.Discount});
			}
		}
		public async Task<List<ItemShopViewModel>> GetAllItems()
		{
			using(IDbConnection dbDapper = new SqlConnection(connectionString))
			{
				var result = await dbDapper.QueryAsync<Item>("SELECT * FROM Items");
				List<Item> items = result.ToList();


				List<ItemShopViewModel> itemShopList = new List<ItemShopViewModel>();
				foreach (var item in items)
				{
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
					itemShopList.Add(itemShop);
				}
				return itemShopList;
			}
		}
		public async Task HideItem(int itemId)
		{
			using (IDbConnection dbDapper = new SqlConnection(connectionString))
			{
				var query = "UPDATE Items SET IsHide = 1 WHERE Id = @itemId";
				await dbDapper.ExecuteAsync(query, new { itemId });
			}
		}
		public async Task HideItemOff(int itemId)
		{
			using (IDbConnection dbDapper = new SqlConnection(connectionString))
			{
				var query = "UPDATE Items SET IsHide = 0 WHERE Id = @itemId";
				await dbDapper.ExecuteAsync(query, new { itemId });
			}
		}

	}
}
