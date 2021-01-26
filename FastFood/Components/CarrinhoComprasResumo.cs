using FastFood.Models;
using FastFood.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FastFood.Components
{
    [ViewComponent]
    public class CarrinhoComprasResumo : ViewComponent
    {
        private readonly CarrinhoCompras _carrinhoCompras;

        public CarrinhoComprasResumo(CarrinhoCompras carrinhoCompras)
        {
            _carrinhoCompras = carrinhoCompras;
        }

        public IViewComponentResult Invoke()
        {
            //var itens = _carrinhoCompras.GetCarrinhoCompraItens();
            var itens = new List<CarrinhoCompraItem>
            {
                new CarrinhoCompraItem(),
                new CarrinhoCompraItem()
            };
            _carrinhoCompras.CarrinhoCompraItens = itens;

            var carrinhoComprasViewModel = new CarrinhoComprasViewModel
            {
                CarrinhoCompra = _carrinhoCompras,
                CarrinhoCompraTotal = _carrinhoCompras.GetCarrinhoCompraTotal()
            };

            return View(carrinhoComprasViewModel);
        }
    }
}
