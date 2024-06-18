using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projeto_clientes.Models
{
    [Table("PessoasFisicas")]
    public class PessoaFisica
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string? NomeCompleto { get; set; }

        [Required]
        [MaxLength(11)]
        public string? CPF { get; set; }

        public DateTime? DataDeNascimento { get; set; }

        public string? Endereco { get; set; }

        [Required]
        public ICollection<Contato>? Contatos { get; set; } = new List<Contato>();
    }
}
