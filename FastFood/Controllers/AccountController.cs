using FastFood.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
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
                        return RedirectToAction(nameof(Index), "Home");

                    return RedirectToAction(viewModel.ReturnUrl);
                }
            }

            ModelState.AddModelError(String.Empty, "Credencias inválidas ou não localizadas");
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser() { UserName = viewModel.Username };
                var result = await _userManager.CreateAsync(user, viewModel.Password);

                if (result.Succeeded)
                    return RedirectToAction(nameof(Index), "Home");

                var errors = result.Errors.Select(x => x.Description).Aggregate((concat, str) => $"{concat} {str}");
                ModelState.AddModelError(String.Empty, errors);
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Index), "Home");
        }
    }
}
