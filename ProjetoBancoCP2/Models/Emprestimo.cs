using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoBancoCP2.Models
{
    public class Emprestimo : Produto
    {
        [Required]
        [Column("VL_SOLICITADO")]
        public decimal ValorSolicitado { get; set; }

        [Required]
        [Column("TX_JUROS")]
        public decimal TaxaJuros { get; set; }

        [Required]
        [Column("NR_PRAZO_MESES")]
        public int PrazoMeses { get; set; }

        [Column("VL_PARCELA")]
        public decimal ValorParcela { get; set; }
    }
}