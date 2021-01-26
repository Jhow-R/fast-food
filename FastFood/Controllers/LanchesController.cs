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
    }
}
