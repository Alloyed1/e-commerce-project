using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using e_commerce_project.Models;
using e_commerce_project.Repositories;
using e_commerce_project.ViewModel.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace e_commerce_project.Controllers
{
	public class AdminController : Controller
	{
		private IAdminRepository adminRepository;
		public AdminController(IAdminRepository adminRepository)
		{
			this.adminRepository = adminRepository;
		}
		public IActionResult Index()
		{
			return View();
		}
		[HttpGet]
		public IActionResult AddItem()
		{
			return View();
		}

		[HttpPost]
		public async Task AddItemInShop(string Name, string About, string Price, Dictionary<int, int> ItemSizes, string[] ItemPreimush, string[] ItemImagesLinks)
		{
			Item item = new Item();
			item.Name = Name;
			item.About = About;
			item.Price = Convert.ToDouble(Price);
			item.AdvantagesArray = JsonConvert.SerializeObject(ItemPreimush);
			item.SizesDictionary = JsonConvert.SerializeObject(ItemSizes);
			item.Discount = 0;
			item.AddItemTime = DateTime.Now;
			item.ItemImagesLinks = JsonConvert.SerializeObject(ItemImagesLinks);

			await adminRepository.AddItem(item);
		}
		[HttpGet]
		public async Task<List<ItemShopViewModel>> GetAllItemsInShop()
		{
			return await adminRepository.GetAllItems();
		}
		[HttpGet]
		public async Task<ItemShopViewModel> GetItemInShop(int id)
		{
			return await adminRepository.GetItem(id);
		}
		[HttpGet]
		public IActionResult AllItems()
		{
			return View();
		}
		[HttpPost]
		public async Task HideItem(int id)
		{
			await adminRepository.HideItem(id);
		}
		[HttpPost]
		public async Task HideItemOff(int id)
		{
			await adminRepository.HideItemOff(id);
		}
		[HttpGet]
		public IActionResult EditItem(int itemId)
		{
			return View(itemId);
		}
		[HttpPost]
		public async Task EditItem(string Name, string About, string Price, Dictionary<int, int> ItemSizes, string[] ItemPreimush, string ItemId, string[] ItemImagesLinks)
		{
			Item item = new Item();
			item.Name = Name;
			item.About = About;
			item.Price = Convert.ToDouble(Price);
			item.AdvantagesArray = JsonConvert.SerializeObject(ItemPreimush);
			item.SizesDictionary = JsonConvert.SerializeObject(ItemSizes);
			item.Discount = 0;
			item.AddItemTime = DateTime.Now;
			item.Id = Convert.ToInt32(ItemId);
			item.ItemImagesLinks = JsonConvert.SerializeObject(ItemImagesLinks);

			await adminRepository.EditItem(item);
		}

	}
}
