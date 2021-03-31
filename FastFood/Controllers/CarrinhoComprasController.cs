using FastFood.Data.Repository.Interfaces;
using FastFood.Models;
using FastFood.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace FastFood.Controllers
{
    public class CarrinhoComprasController : Controller
    {
        private readonly ILancheRepository _lancheRepository;
        private readonly CarrinhoCompras _carrinhoCompra;

        public CarrinhoComprasController(ILancheRepository lancheRepository,
                                        CarrinhoCompras carrinhoCompra)
        {
            _lancheRepository = lancheRepository;
            _carrinhoCompra = carrinhoCompra;
        }

        public IActionResult Index()
        {
            var itens = _carrinhoCompra.GetCarrinhoCompraItens();
            _carrinhoCompra.CarrinhoCompraItens = itens;

            var model = new CarrinhoComprasViewModel
            {
                CarrinhoCompra = _carrinhoCompra,
                CarrinhoCompraTotal = _carrinhoCompra.GetCarrinhoCompraTotal()
            };

            return View(model);
        }

        [Authorize]
        public RedirectToActionResult AdicionarItemNoCarrinhoCompra(int lancheId)
        {
            var lancheSelecionado = _lancheRepository.Lanches.FirstOrDefault(l => l.Id == lancheId);

            if (lancheSelecionado is not null)
                _carrinhoCompra.AdicionarAoCarrinho(lancheSelecionado);

            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public IActionResult RemoverItemDoCarrinhoCompra(int lancheId)
        {
            var lancheSelecionado = _lancheRepository.Lanches.FirstOrDefault(l => l.Id == lancheId);

            if (lancheSelecionado is not null)
                _carrinhoCompra.RemoverDoCarrinho(lancheSelecionado);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult List()
        {
            return RedirectToAction(nameof(Index));
        }
    }
}
