using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoBancoCP2.Models
{
    public class PessoaFisica : Cliente
    {
        [Required]
        [StringLength(11)]
        [Column("CPF")]
        public string Cpf { get; set; }

        [DataType(DataType.Date)]
        [Column("DT_NASCIMENTO")]
        public DateTime DataNascimento { get; set; }
    }
}