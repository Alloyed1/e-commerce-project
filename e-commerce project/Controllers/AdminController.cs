using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using e_commerce_project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace e_commerce_project.Controllers
{
	[Authorize(Roles ="Admin")]
	public class AdminController : Controller
	{
		public AdminController()
		{

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
		public IActionResult AddItem(string Name, string About, string Price, Dictionary<int, int> ItemSizes, string[] ItemPreimush, IFormFile Image)
		{
			Item item = new Item();
			item.Name = Name;
			item.About = About;
			item.Price = Convert.ToDouble(Price);
			item.AdvantagesArray = JsonConvert.SerializeObject( ItemPreimush);
			item.SizesDictionary = JsonConvert.SerializeObject(ItemSizes);
			item.Discount = 0;
			return View();

		}

	}
}
