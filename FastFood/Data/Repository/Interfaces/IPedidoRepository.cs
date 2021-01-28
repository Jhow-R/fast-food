using FastFood.Models;

namespace FastFood.Data.Repository.Interfaces
{
    public interface IPedidoRepository
    {
        void CriarPedido(Pedido pedido);
    }
}
