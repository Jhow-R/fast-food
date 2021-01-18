using FastFood.Models;
using System.Collections.Generic;

namespace FastFood.Data.Repository.Interfaces
{
    public interface ICategoriaRepository
    {
        IEnumerable<Categoria> Categorias { get; }
    }
}
