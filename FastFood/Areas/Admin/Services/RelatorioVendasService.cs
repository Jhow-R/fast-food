using FastFood.Data.Context;
using FastFood.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFood.Areas.Admin.Services
{
    public class RelatorioVendasService
    {
        private readonly AppDbContext context;
        public RelatorioVendasService(AppDbContext _context)
        {
            context = _context;
        }

        public async Task<IEnumerable<Pedido>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            var resultado = from obj in context.Pedidos select obj;

            if (minDate.HasValue)
                resultado = resultado.Where(x => x.PedidoEnviado >= minDate.Value);
            
            if (maxDate.HasValue)
                resultado = resultado.Where(x => x.PedidoEnviado <= maxDate.Value);

            return await resultado
                .Include(l => l.PedidoItens)
                .ThenInclude(l=> l.Lanche)
                .OrderByDescending(x => x.PedidoEnviado)
                .ToListAsync();
        }
    }
}
