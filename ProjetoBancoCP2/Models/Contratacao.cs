using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoBancoCP2.Models
{
    [Table("TB_CONTRATACAO")]
    public class Contratacao
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID_CONTRATACAO")]
        public int IdContratacao { get; set; }

        [Column("ID_CLIENTE")]
        public int IdCliente { get; set; }

        [ForeignKey("IdCliente")]
        public Cliente Cliente { get; set; }

        [Column("ID_PRODUTO")]
        public int IdProduto { get; set; }

        [ForeignKey("IdProduto")]
        public Produto Produto { get; set; }

        [Required]
        [StringLength(50)]
        [Column("STATUS")]
        public string Status { get; set; } = "PENDENTE";

        [Column("DT_SOLICITACAO")]
        public DateTime DtSolicitacao { get; set; } = DateTime.Now;
    }
}