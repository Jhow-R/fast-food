using FastFood.Models;
using System.Collections.Generic;

namespace FastFood.Data.Repository.Interfaces
{
    public interface ILancheRepository
    {
        IEnumerable<Lanche> Lanches{ get; }
        IEnumerable<Lanche> LanchesFavoritos { get; }
        Lanche GetLanchesById(int lancheId);
    }
}
