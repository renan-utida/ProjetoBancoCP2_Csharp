using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoBancoCP2.Models
{
    [Table("TB_PRODUTO")]
    public abstract class Produto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID_PRODUTO")]
        public int IdProduto { get; set; }

        [Required]
        [StringLength(100)]
        [Column("NM_PRODUTO")]
        public string NmProduto { get; set; }
    }
}