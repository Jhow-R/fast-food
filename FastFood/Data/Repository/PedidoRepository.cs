using FastFood.Data.Context;
using FastFood.Data.Repository.Interfaces;
using FastFood.Models;
using System;

namespace FastFood.Data.Repository
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly AppDbContext _context;
        private readonly CarrinhoCompras _carrinhoCompras;

        public PedidoRepository(AppDbContext context, CarrinhoCompras carrinhoCompras)
        {
            _context = context;
            _carrinhoCompras = carrinhoCompras;
        }

        public void CriarPedido(Pedido pedido)
        {
            pedido.PedidoEnviado = DateTime.Now; 
            _context.Pedidos.Add(pedido);

            var carrinhoCompraItens = _carrinhoCompras.CarrinhoCompraItens;
            foreach (var item in carrinhoCompraItens)
            {
                var pedidoDetalhe = new PedidoDetalhe
                {
                    Quantidade = item.Quantidade,
                    LancheId = item.Lanche.Id,
                    PedidoId = pedido.Id,
                    Preco = item.Lanche.Preco
                };

                _context.PedidoDetalhes.Add(pedidoDetalhe);
            }

            _context.SaveChanges();
        }
    }
}
