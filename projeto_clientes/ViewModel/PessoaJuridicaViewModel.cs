using projeto_clientes.Models;

namespace projeto_clientes.ViewModels
{
    public class PessoaJuridicaViewModel
    {
        public string? RazaoSocial { get; set; }

        public string? CNPJ { get; set; }

        public string? NomeFantasia { get; set; }

        public string? Endereco { get; set; }

        public ICollection<Contato>? Contatos { get; set; } = new List<Contato>();
    }
}
