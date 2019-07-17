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
	}
	public class ShopRepository: IShopRepository
	{
		string connectionString;
		public ShopRepository(IConfiguration configuration)
		{
			connectionString = configuration.GetConnectionString("DefaultConnection");
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
