using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace e_commerce_project.ViewModel.Account
{
	public class LoginViewModel
	{
		[Required]
		public string Email { get; set; }

		public bool RememberMe { get; set; }

		[Required(AllowEmptyStrings = false)]
		[DataType(DataType.Password)]
		public string Password { get; set; }
	}
}
