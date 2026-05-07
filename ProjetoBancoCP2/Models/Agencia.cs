using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoBancoCP2.Models
{
    [Table("TB_AGENCIA")]
    public class Agencia
    {
        [Key]
        [Column("ID_AGENCIA")]
        public int IdAgencia { get; set; }

        [Required]
        [StringLength(100)]
        [Column("NM_AGENCIA")]
        public string NmAgencia { get; set; }

        [Required]
        [StringLength(8)]
        [Column("CEP")]
        public string Cep { get; set; }

        [Required]
        [StringLength(200)]
        [Column("DS_ENDERECO")]
        public string DsEndereco { get; set; }
    }
}