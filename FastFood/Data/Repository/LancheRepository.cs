using FastFood.Data.Context;
using FastFood.Data.Repository.Interfaces;
using FastFood.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FastFood.Data.Repository
{
    public class LancheRepository : ILancheRepository
    {
        private readonly AppDbContext _context;

        public LancheRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Lanche> Lanches => _context.Lanches.Include(l => l.Categoria);

        public IEnumerable<Lanche> LanchesFavoritos => _context.Lanches
            .Where(l => l.IsLancheFavorito == true)
            .Include(l => l.Categoria);

        public Lanche GetLanchesById(int lancheId) => _context.Lanches
            .Where(l => l.Id == lancheId)
            .FirstOrDefault();
    }
}
