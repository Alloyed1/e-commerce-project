using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using e_commerce_project.Models;

namespace e_commerce_project.Repositories
{
	public interface ICategoryRepository
	{
		Task AddCategory(string categoryName);
		Task<List<Category>> GetAllCaregories();
		Task<Category> GetCategory(int categoryId);
	}
	public class CategoryRepository: ICategoryRepository
	{
		string connectionString;
		public CategoryRepository(IConfiguration configuration)
		{
			connectionString = configuration.GetConnectionString("DefaultConnection");
		}

		public async Task AddCategory(string categoryName)
		{
			using (IDbConnection dbDapper = new SqlConnection(connectionString))
			{
				var query = "INSERT INTO Categories (CategoryName) VALUES(@categoryName)";
				await dbDapper.ExecuteAsync(query, new { categoryName });
			}
		}
		public async Task<List<Category>> GetAllCaregories()
		{
			using(IDbConnection dbDapper = new SqlConnection(connectionString))
			{
				var query = "SELECT * FROM Categories";
				var result = await dbDapper.QueryAsync<Category>(query);
				return result.ToList();
			}
			
		}
		public async Task<Category> GetCategory(int categoryId)
		{
			using(IDbConnection dbDapper = new SqlConnection(connectionString))
			{
				return await dbDapper.QueryFirstOrDefaultAsync<Category>("SELECT * FROM Categories WHERE Id = @categoryId", new { categoryId });
			}
		}
	}
}
