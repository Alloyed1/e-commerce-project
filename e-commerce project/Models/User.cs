﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_commerce_project.Models
{
	public class User : IdentityUser
	{
		public string FirstName { get; set; }
		public string Address { get; set; }
		public string LastName { get; set; }
		public int Age { get; set; }
		public string Sex { get; set; }
	}
}
