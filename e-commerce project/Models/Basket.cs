using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace e_commerce_project.Models
{
	public class Basket
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Key]
		public string UserId { get; set; }
		public User User { get; set; }
		public string BasketItemsList { get; set; }
	}
}
