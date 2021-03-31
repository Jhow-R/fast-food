using FastFood.Data.Repository.Interfaces;
using FastFood.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace FastFood.Controllers
{
    public class PedidosController : Controller
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly CarrinhoCompras _carrinhoCompras;

        public PedidosController(IPedidoRepository pedidoRepository, CarrinhoCompras carrinhoCompras)
        {
            _pedidoRepository = pedidoRepository;
            _carrinhoCompras = carrinhoCompras;
        }

        [Authorize]
        public IActionResult Checkout()
        {
            var items = _carrinhoCompras.GetCarrinhoCompraItens();
            _carrinhoCompras.CarrinhoCompraItens = items;

            if (_carrinhoCompras.CarrinhoCompraItens.Count == 0)
                ModelState.AddModelError(String.Empty, "Seu carrinho está vazio");

            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Checkout(Pedido pedido)
        {
            var items = _carrinhoCompras.GetCarrinhoCompraItens();
            _carrinhoCompras.CarrinhoCompraItens = items;

            if (_carrinhoCompras.CarrinhoCompraItens.Count == 0)
                ModelState.AddModelError(String.Empty, "Seu carrinho está vazio");

            if (ModelState.IsValid)
            {
                _pedidoRepository.CriarPedido(pedido);

                ViewBag.CheckoutCompletoMensagem = "Pedido feito com sucesso!";
                ViewBag.TotalPedido = _carrinhoCompras.GetCarrinhoCompraTotal();

                _carrinhoCompras.LimparCarrinho();
                
                return View("~/Views/Pedidos/CheckoutCompleto.cshtml", pedido);
            }

            return View(pedido);
        }
    }
}
