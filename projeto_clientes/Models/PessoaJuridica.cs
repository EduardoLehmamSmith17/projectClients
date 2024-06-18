using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projeto_clientes.Models
{
    [Table("PessoasJuridicas")]
    public class PessoaJuridica
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string? RazaoSocial { get; set; }

        [Required]
        [MaxLength(14)]
        public string? CNPJ { get; set; }

        [MaxLength(200)]
        public string? NomeFantasia { get; set; }

        public string? Endereco { get; set; }

        [Required]
        public ICollection<Contato>? Contatos { get; set; } = new List<Contato>();
    }
}
