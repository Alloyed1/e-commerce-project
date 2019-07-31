using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace e_commerce_project.Models
{
	public class ItemsOrders
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public int OrderId { get; set; }
		public Order Order { get; set; }

		public int ItemId { get; set; }
		public Item Item { get; set; }

		public int Size { get; set; }
		public int Count { get; set; }
	}
}
