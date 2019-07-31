﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace e_commerce_project.Models
{
	public class Item
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string Name { get; set; }
		public double Price { get; set; }
		public string About { get; set; }
		public int CountOfPurchases { get; set; }
		public DateTime AddItemTime { get; set; }
		public string SizesDictionary { get; set; }
		public string AdvantagesArray { get; set; }
		
		public byte IsDelete { get; set; }
		public byte IsHide { get; set; }

		public string ItemImagesLinks { get; set; }

		public int? Discount { get; set; }

		public int CategoryId { get; set; }
		public Category Category { get; set; }
	}
}
