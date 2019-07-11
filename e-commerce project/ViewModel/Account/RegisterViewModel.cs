using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace e_commerce_project.ViewModel.Account
{
	public class RegisterViewModel
	{
		[Required]
		public string Email { get; set; }

		[Required(AllowEmptyStrings = false)]
		public string FirstName { get; set; }

		[Required(AllowEmptyStrings = false)]
		[DataType(DataType.Password)]
		public string Password { get; set; }
	}
}
