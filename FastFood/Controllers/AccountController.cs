using FastFood.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FastFood.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel()
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (ModelState.IsValid is false)
                return View(viewModel);

            var user = await _userManager.FindByNameAsync(viewModel.Username);

            if (user is not null)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    user, viewModel.Password, false, false);

                if (result.Succeeded)
                {
                    if (String.IsNullOrEmpty(viewModel.ReturnUrl))
                        return RedirectToAction(nameof(Index), nameof(HomeController));

                    return RedirectToAction(viewModel.ReturnUrl);
                }
            }

            ModelState.AddModelError(String.Empty, "Credencias inválidas ou não localizadas");
            return View(viewModel);
        }
    }
}
