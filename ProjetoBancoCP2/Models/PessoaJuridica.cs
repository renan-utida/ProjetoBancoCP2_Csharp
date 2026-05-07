using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoBancoCP2.Models
{
    public class PessoaJuridica : Cliente
    {
        [Required]
        [StringLength(14)]
        [Column("CNPJ")]
        public string Cnpj { get; set; }

        [Required]
        [StringLength(200)]
        [Column("RAZAO_SOCIAL")]
        public string RazaoSocial { get; set; }
    }
}