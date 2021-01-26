using FastFood.Data.Repository.Interfaces;
using FastFood.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FastFood.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ILancheRepository _lancheRepository;

        public HomeController(ILogger<HomeController> logger, ILancheRepository lancheRepository)
        {
            _logger = logger;
            _lancheRepository = lancheRepository;
        }

        public IActionResult Index()
        {
            var homeViewModel = new HomeViewModel
            {
                LanchesFavoritos = _lancheRepository.LanchesFavoritos
            };

            return View(homeViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
