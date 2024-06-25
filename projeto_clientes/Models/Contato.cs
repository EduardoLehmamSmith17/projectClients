using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projeto_clientes.Models
{
    [Table("Contatos")] 
    public class Contato
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Telefone { get; set; }
    }
}
