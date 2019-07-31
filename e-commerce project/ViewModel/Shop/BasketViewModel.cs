using e_commerce_project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_commerce_project.ViewModel.Shop
{
	public class BasketViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string About { get; set; }
		public double Price { get; set; }
		public double Discount { get; set; }
		public int Size { get; set; }
		public int Count { get; set; }
		public string ImageUrl { get; set; }
	}
}
