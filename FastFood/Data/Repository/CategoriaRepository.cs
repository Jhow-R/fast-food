using FastFood.Data.Context;
using FastFood.Data.Repository.Interfaces;
using FastFood.Models;
using System.Collections.Generic;
using System.Linq;

namespace FastFood.Data.Repository
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly AppDbContext _context;

        public CategoriaRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Categoria> Categorias => _context.Categorias;
    }
}
