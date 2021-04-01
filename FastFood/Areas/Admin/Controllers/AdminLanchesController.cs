using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FastFood.Data.Context;
using FastFood.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using ReflectionIT.Mvc.Paging;

namespace FastFood.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminLanchesController : Controller
    {
        private readonly AppDbContext _context;

        public AdminLanchesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Lanches
        /*public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Lanches.Include(l => l.Categoria);
            return View(await appDbContext.ToListAsync());
        }*/

        public async Task<IActionResult> Index(string filter, int pageindex = 1, string sort = "Nome")
        {
            var resultado = _context.Lanches
                .Include(l => l.Categoria)
                .AsQueryable();

            if (!String.IsNullOrWhiteSpace(filter))
                resultado = resultado.Where(p => p.Nome.Contains(filter));

            var model = await PagingList.CreateAsync(resultado, 3, pageindex, sort, "Nome");
            model.RouteValue = new RouteValueDictionary { { "filter", filter } };

            return View(model);
        }

        // GET: Admin/Lanches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lanche = await _context.Lanches
                .Include(l => l.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lanche == null)
            {
                return NotFound();
            }

            return View(lanche);
        }

        // GET: Admin/Lanches/Create
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Id");
            return View();
        }

        // POST: Admin/Lanches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,DescricaoCurta,DescricaoDetalhada,Preco,ImagemUrl,ImagemThumbnailUrl,IsLancheFavorito,EmEstoque,CategoriaId")] Lanche lanche)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lanche);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Id", lanche.CategoriaId);
            return View(lanche);
        }

        // GET: Admin/Lanches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lanche = await _context.Lanches.FindAsync(id);
            if (lanche == null)
            {
                return NotFound();
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Id", lanche.CategoriaId);
            return View(lanche);
        }

        // POST: Admin/Lanches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,DescricaoCurta,DescricaoDetalhada,Preco,ImagemUrl,ImagemThumbnailUrl,IsLancheFavorito,EmEstoque,CategoriaId")] Lanche lanche)
        {
            if (id != lanche.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lanche);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LancheExists(lanche.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Id", lanche.CategoriaId);
            return View(lanche);
        }

        // GET: Admin/Lanches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lanche = await _context.Lanches
                .Include(l => l.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lanche == null)
            {
                return NotFound();
            }

            return View(lanche);
        }

        // POST: Admin/Lanches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lanche = await _context.Lanches.FindAsync(id);
            _context.Lanches.Remove(lanche);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LancheExists(int id)
        {
            return _context.Lanches.Any(e => e.Id == id);
        }
    }
}
