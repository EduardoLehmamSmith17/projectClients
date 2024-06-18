using projeto_clientes.Models;
using System.ComponentModel.DataAnnotations;

namespace projeto_clientes.ViewModels
{
    public class PessoaFisicaViewModel
    {
        public string? NomeCompleto { get; set; }

        public string? CPF { get; set; }

        public DateTime? DataDeNascimento { get; set; }

        public string? Endereco { get; set; }

        [Required]
        public ICollection<Contato>? Contatos { get; set; } = new List<Contato>();
    }
}
