using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using e_commerce_project.Models;
using e_commerce_project.ViewModel.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace e_commerce_project.Controllers
{
	public class AccountController : Controller
	{
		private SignInManager<User> signInManager;
		//private IUserRepository userRepository;
		private UserManager<User> userManager;
		private RoleManager<IdentityRole> roleManager;
		public AccountController(SignInManager<User> signInManager, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
		{
			this.signInManager = signInManager;
			this.userManager = userManager;
			this.roleManager = roleManager;
		}
		[HttpGet]
		public IActionResult ChatPage()
		{
			return View();
		}
		public IActionResult Profile()
		{
			if (User.Identity.IsAuthenticated)
			{
				return View();
			}
			else
				return RedirectToAction("Auth", "Account");

		}


		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (ModelState.IsValid)
			{
				User user = new User { Email = model.Email, UserName = model.Email, FirstName = model.FirstName };
				// добавляем пользователя
				var result = await userManager.CreateAsync(user, model.Password);
				if (result.Succeeded)
				{
					await roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
					if (user.Email == "jenya.moxov@gmail.com")
						await userManager.AddToRoleAsync(user, "Admin");

					// установка куки
					await signInManager.SignInAsync(user, true);
					return RedirectToAction("Index", "Home");
				}
				else
				{
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError(string.Empty, error.Description);
					}
				}
			}
			return View(model);
		}


		[HttpGet]
		public IActionResult Auth()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Auth(LoginViewModel model)
		{
			var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
			if (result.Succeeded)
			{
				return RedirectToAction("Index", "Home");
			}
			else
			{
				ModelState.AddModelError("", "Неправильный логин и (или) пароль");
			}
			return View(model);
		}

	}
}
