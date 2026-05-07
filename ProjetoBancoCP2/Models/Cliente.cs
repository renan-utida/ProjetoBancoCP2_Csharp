using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoBancoCP2.Models
{
    [Table("TB_CLIENTE")]
    public abstract class Cliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID_CLIENTE")]
        public int IdCliente { get; set; }

        [Required]
        [StringLength(100)]
        [Column("NM_CLIENTE")]
        public string NmCliente { get; set; }

        [Column("ID_AGENCIA")]
        public int IdAgencia { get; set; }

        [ForeignKey("IdAgencia")]
        public Agencia? Agencia { get; set; }
    }
}