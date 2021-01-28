using FastFood.Data.Repository.Interfaces;
using FastFood.Models;
using FastFood.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FastFood.Controllers
{
    public class LanchesController : Controller
    {
        private readonly ILancheRepository _lancheRepository;
        private readonly ICategoriaRepository _categoriaRepository;

        public LanchesController(ILancheRepository lancheRepository, ICategoriaRepository categoriaRepository)
        {
            _lancheRepository = lancheRepository;
            _categoriaRepository = categoriaRepository;
        }

        public IActionResult List(string categoria)
        {
            ViewBag.Lanche = "Lanches";
            ViewData["Lanche"] = "Lanches";

            string categoriaAtual = String.Empty;
            IEnumerable<Lanche> lanches;

            if (String.Equals("Normal", categoria, StringComparison.OrdinalIgnoreCase) || String.Equals("Natural", categoria, StringComparison.OrdinalIgnoreCase))
            {
                lanches = _lancheRepository.Lanches
                    .Where(l => l.Categoria.Nome.Equals(categoria, StringComparison.OrdinalIgnoreCase))
                    .OrderBy(l => l.Nome);

                categoriaAtual = Char.ToUpper(categoria[0]) + categoria.Substring(1);
            }
            else
            {
                lanches = _lancheRepository.Lanches.OrderBy(l => l.Id);
                categoriaAtual = "Todos os lanches";
            }

            var lanchesListViewModel = new LanchesListViewModel()
            {
                Lanches = lanches,
                CategoriaAtual = categoriaAtual
            };

            return View(lanchesListViewModel);
        }

        public IActionResult Details(int lancheId)
        {
            var lanche = _lancheRepository.Lanches.FirstOrDefault(l => l.Id.Equals(lancheId));

            if (lanche is null)
                return View("~/Views/Error/Error.cshtml");

            return View(lanche);
        }

        public IActionResult Search(string searchString)
        {
            IEnumerable<Lanche> lanches;

            if (String.IsNullOrEmpty(searchString) is false)
                lanches = _lancheRepository.Lanches.Where(l => l.Nome.Contains(searchString, StringComparison.InvariantCultureIgnoreCase));
            else
                lanches = _lancheRepository.Lanches.OrderBy(l => l.Id);

            var viewModel = new LanchesListViewModel
            {
                Lanches = lanches,
                CategoriaAtual = lanches.Count().Equals(default(int)) 
                ? "Nenhum lanche encontrado" 
                : "Todos os lanches"
            };

            return View("~/Views/Lanches/List.cshtml", viewModel);
        }
    }
}
