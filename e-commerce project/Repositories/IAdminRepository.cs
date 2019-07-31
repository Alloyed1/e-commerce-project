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
		Task EditItem(Item item);
		Task<List<ItemShopViewModel>> GetAllItems();
		Task HideItem(int itemId);
		Task HideItemOff(int itemId);
		Task<ItemShopViewModel> GetItem(int itemId);
	}
	public class AdminRepository: IAdminRepository
	{
		readonly string connectionString;
		ICategoryRepository categoryRepository;
		ApplicationContext dbContext;
		public AdminRepository(IConfiguration configuration, ICategoryRepository categoryRepository, ApplicationContext dbContext)
		{
			connectionString = configuration.GetConnectionString("DefaultConnection");
			this.categoryRepository = categoryRepository;
			this.dbContext = dbContext;
		}

		public async Task AddItem(Item item)
		{
			//await dbContext.Items.AddAsync(item);
			using (IDbConnection dbDapper = new SqlConnection(connectionString))
			{
				string query = "INSERT INTO Items (Name, Price, About, CountOfPurchases, AddItemTime, SizesDictionary, AdvantagesArray, Discount, ItemImagesLinks, CategoryId) VALUES(@Name, @Price, @About,@CountOfPurchases, @AddItemTime, @SizesDictionary, @AdvantagesArray, @Discount, @ItemImagesLinks, @CategoryId)";

				await dbDapper.ExecuteAsync(query, new { item.Name, item.Price, item.About, item.CountOfPurchases, item.AddItemTime, item.SizesDictionary, item.AdvantagesArray, item.Discount, item.ItemImagesLinks, item.CategoryId });
			}
		}
		public async Task EditItem(Item item)
		{
			using (IDbConnection dbDapper = new SqlConnection(connectionString))
			{
				var query = @"UPDATE Items SET 
										Name = @Name, Price = @Price, About = @About, CountOfPurchases = @CountOfPurchases, AddItemTime = @AddItemTime, SizesDictionary = @SizesDictionary, ItemImagesLinks = @ItemImagesLinks, CategoryId = @CategoryId WHERE Id = @Id";

				await dbDapper.ExecuteAsync(query, new { item.Name, item.Id, item.Price, item.About, item.CountOfPurchases, item.AddItemTime, item.SizesDictionary, item.ItemImagesLinks, item.CategoryId});
			}
		}
		public async Task<List<ItemShopViewModel>> GetAllItems()
		{
			using(var dbDapper = new SqlConnection(connectionString))
			{
				var result = (await dbDapper.QueryAsync<Item>("SELECT * FROM Items"));
				var items = result.ToList();

				List<Category> categories = await categoryRepository.GetAllCaregories();


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
					itemShop.Category = categories.Where(i => i.Id == Convert.ToInt32(item.CategoryId)).FirstOrDefault();
					itemShop.SizesDictionary = JsonConvert.DeserializeObject<Dictionary<int, int>>(item.SizesDictionary);
					itemShop.AdvantagesArray = JsonConvert.DeserializeObject<string[]>(item.AdvantagesArray);
					itemShop.ItemImagesLinks = JsonConvert.DeserializeObject<string[]>(item.ItemImagesLinks);

					itemShopList.Add(itemShop);
				}
				return itemShopList;
			}
		}
		public async Task<ItemShopViewModel> GetItem(int itemId)
		{
			using (IDbConnection dbDapper = new SqlConnection(connectionString))
			{
				var result = await dbDapper.QueryAsync<Item>("SELECT * FROM Items WHERE Id = @itemId", new { itemId});
				Item item = result.FirstOrDefault();


				ItemShopViewModel itemShop = new ItemShopViewModel();

				itemShop.Id = item.Id;
				itemShop.Name = item.Name;
				itemShop.About = item.About;
				itemShop.Price = item.Price;
				itemShop.Discount = item.Discount;
				itemShop.Category = await categoryRepository.GetCategory(Convert.ToInt32(item.CategoryId));
				itemShop.IsDelete = item.IsDelete;
				itemShop.IsHide = item.IsHide;
				itemShop.SizesDictionary = JsonConvert.DeserializeObject<Dictionary<int, int>>(item.SizesDictionary);
				itemShop.AdvantagesArray = JsonConvert.DeserializeObject<string[]>(item.AdvantagesArray);
				itemShop.ItemImagesLinks = JsonConvert.DeserializeObject<string[]>(item.ItemImagesLinks);

				return itemShop;
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
