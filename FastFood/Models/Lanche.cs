using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastFood.Models
{
    public class Lanche
    {
        // EF automáticamente entende a propriedade Id (ou LancheId) como chave primária
        public int Id { get; set; }

        [StringLength(100)]
        public string Nome { get; set; }

        [StringLength(100)]
        public string DescricaoCurta { get; set; }
        
        [StringLength(255)]
        public string DescricaoDetalhada{ get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Preco { get; set; }
        
        [StringLength(200)]
        public string ImagemUrl { get; set; }
        
        [StringLength(200)]
        public string ImagemThumbnailUrl { get; set; }
        
        public bool IsLancheFavorito { get; set; }
        public bool EmEstoque { get; set; }
        public int CategoriaId { get; set; }

        // O modificador virtual é utilizado pelo EF para fazer o Lazy Loading: https://pt.stackoverflow.com/questions/52908/qual-a-diferen%C3%A7a-entre-usar-propriedade-virtual-ou-n%C3%A3o-no-ef
        public virtual Categoria Categoria { get; set; }
    }
}
