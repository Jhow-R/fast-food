using FastFood.Data.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace FastFood.Components
{
    public class CategoriaMenu : ViewComponent
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public CategoriaMenu(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public IViewComponentResult Invoke() => View(_categoriaRepository.Categorias.OrderBy(c => c.Nome));
    }
}

