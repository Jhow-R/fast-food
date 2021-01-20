using FastFood.Data.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FastFood.Models
{
    public class CarrinhoCompra
    {
        private const string CarrinhoIdSessionKey = "CarrinhoId";
        private readonly AppDbContext _context;

        public string Id { get; set; }
        public List<CarrinhoCompraItem> CarrinhoCompraItens { get; set; }

        private CarrinhoCompra(AppDbContext context)
        {
            _context = context;
        }

        public static CarrinhoCompra GetCarrinho(IServiceProvider service)
        {
            // Define uma sessão acessando o contexto atual (tem que registrar em IServicesCollection)
            ISession session = service.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;

            // Obtém um serviço do tipo do nosso contexto
            AppDbContext context = service.GetService<AppDbContext>();

            // Obtém ou gera o Id do carrinho
            var carrinhoId = session.GetString(CarrinhoIdSessionKey) ?? Guid.NewGuid().ToString();

            // Atribui Id do carrinho na sessão
            session.SetString(CarrinhoIdSessionKey, carrinhoId);

            // Retorna o carrinho com o contexto atual e o Id atribuído ou obtido
            return new CarrinhoCompra(context)
            {
                Id = carrinhoId
            };
        }

        public void AdicionarAoCarrinho(Lanche lanche, int quantidade)
        {
            // Obtém o lanche de um carrinho especificado
            var carrinhoCompraItem =
                    _context.CarrinhoCompraItens.SingleOrDefault(
                        s => s.Lanche.Id.Equals(lanche.Id) && s.CarrinhoCompraId.Equals(Id));

            // Verifica se o carrinho existe e cria um caso não existir
            if (carrinhoCompraItem is null)
            {
                carrinhoCompraItem = new CarrinhoCompraItem
                {
                    CarrinhoCompraId = Id,
                    Lanche = lanche,
                    Quantidade = 1
                };

                _context.CarrinhoCompraItens.Add(carrinhoCompraItem);
            }
            else
                carrinhoCompraItem.Quantidade++;
   
            _context.SaveChanges();
        }

        public int RemoverDoCarrinho(Lanche lanche)
        {
            var carrinhoCompraItem =
                    _context.CarrinhoCompraItens.SingleOrDefault(
                        s => s.Lanche.Id.Equals(lanche.Id) && s.CarrinhoCompraId.Equals(Id));

            var quantidadeLocal = default(int);

            if (carrinhoCompraItem is not null)
            {
                if (carrinhoCompraItem.Quantidade > 1)
                {
                    carrinhoCompraItem.Quantidade--;
                    quantidadeLocal = carrinhoCompraItem.Quantidade;
                }
                else
                    _context.CarrinhoCompraItens.Remove(carrinhoCompraItem);
            }

            _context.SaveChanges();

            return quantidadeLocal;
        }

        public void LimparCarrinho()
        {
            var carrinhoItens = _context.CarrinhoCompraItens
                .Where(carrinho => carrinho.CarrinhoCompraId.Equals(Id));

            _context.CarrinhoCompraItens.RemoveRange(carrinhoItens);
            _context.SaveChanges();
        }

        public decimal GetCarrinhoCompraTotal() => _context.CarrinhoCompraItens
            .Where(c => c.CarrinhoCompraId == Id)
            .Select(c => c.Lanche.Preco * c.Quantidade)
            .Sum();
    }
}
