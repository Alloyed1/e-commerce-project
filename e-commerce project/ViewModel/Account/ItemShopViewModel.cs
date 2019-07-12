using e_commerce_project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_commerce_project.ViewModel.Account
{
	public class ItemShopViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public double Price { get; set; }
		public string About { get; set; }
		public int CountOfPurchases { get; set; }
		public DateTime AddItemTime { get; set; }
		public Dictionary<int, int> SizesDictionary { get; set; }
		public string[] AdvantagesArray { get; set; }

		public byte[] FirstImage { get; set; }
		public byte[] SecondImage { get; set; }
		public byte[] ThirdImage { get; set; }

		public int? Discount { get; set; }

		public Brand Brand { get; set; }
		public Category Category { get; set; }
	}
}
