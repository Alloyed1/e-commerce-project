using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using e_commerce_project.Repositories;
using e_commerce_project.ViewModel.Account;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace e_commerce_project.Controllers
{
	public class ShopController : Controller
	{
		IShopRepository shopRepository;
		public ShopController(IShopRepository shopRepository)
		{
			this.shopRepository = shopRepository;
		}
		[HttpGet]
		public IActionResult Index ()
		{
			return View();
		}
		[HttpGet]
		public IActionResult Info(int itemId)
		{
			return View(itemId);
		}
		[HttpGet]
		public async Task<ItemShopViewModel> GetItemInShop(int id)
		{
			return await shopRepository.GetItem(id);
		}
	}
}
