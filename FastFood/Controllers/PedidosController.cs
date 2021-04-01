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
            decimal precoTotalPedido = 0.0m;
            int totalItensPedido = 0;

            var items = _carrinhoCompras.GetCarrinhoCompraItens();
            _carrinhoCompras.CarrinhoCompraItens = items;

            if (_carrinhoCompras.CarrinhoCompraItens.Count == 0)
                ModelState.AddModelError(String.Empty, "Seu carrinho está vazio");

            foreach (var item in items)
            {
                totalItensPedido += item.Quantidade;
                precoTotalPedido += (item.Lanche.Preco * item.Quantidade);
            }

            pedido.TotalItensPedido = totalItensPedido;
            pedido.PedidoTotal = precoTotalPedido;

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
