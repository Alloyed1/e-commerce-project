using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using e_commerce_project.Models;
using e_commerce_project.Repositories;
using e_commerce_project.ViewModel.Account;
using e_commerce_project.ViewModel.Shop;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace e_commerce_project.Controllers
{
	public class ShopController : Controller
	{
		private readonly IShopRepository shopRepository;

		public ShopController(IShopRepository shopRepository)
		{
			this.shopRepository = shopRepository;
		}

		[HttpGet]
		public IActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public IActionResult Info(int itemId)
		{
			return View(itemId);
		}

		[HttpGet]
		public IActionResult Favorite()
		{
			return View();
		}
		[HttpGet]
		public async Task<List<ItemShopViewModel>> GetFavoriteList(string userEmail)
		{
			return await shopRepository.GetFavoriteList(userEmail);
		}

		[HttpDelete]
		public async Task DeleteFromFavorite(string userEmail, int itemId) {
			await shopRepository.DeleteFromFavorite(userEmail, itemId);
		}

		[HttpDelete]
		public async Task DeleteFromBasket(string userEmail, int itemId, int itemSize)
		{
			await shopRepository.DeleteFromBasket(userEmail, itemId, itemSize);
		}
		[HttpGet]
		public async Task<ItemShopViewModel> GetItemInShop(int id)
		{
			return await shopRepository.GetItem(id);
		}
		[HttpPut]
		public async Task<bool> AddToFavorite(int id, string userEmail)
		{
			return await shopRepository.AddToFavorit(userEmail, id);
		}
		[HttpPut]
		public async Task<bool> AddToBasket(int itemId, string userEmail, int size, int count)
		{
			return await shopRepository.AddToBaskets(userEmail, itemId, size, count);
		}
		[HttpGet]
		public async Task<List<BasketViewModel>> GetBasketList(string userEmail)
		{
			return await shopRepository.GetBasketList(userEmail);
		}

		[HttpPut]
		public async Task AddOrder(string userEmail, List<BasketViewModel> items, int sum)
		{
			await shopRepository.AddOrder(userEmail, items, sum);
		}
		[HttpGet]
		public IActionResult Basket()
		{
			return View();
		}
	}
}
