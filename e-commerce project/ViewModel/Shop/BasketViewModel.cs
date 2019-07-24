using e_commerce_project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_commerce_project.ViewModel.Shop
{
	public class BasketViewModel
	{
		public Item Item { get; set; }
		public int Size { get; set; }

		public int Count { get; set; }
	}
}
