using projeto_clientes.Models;

namespace projeto_clientes.Repositorio.Interfaces
{
    public interface IPessoaJuridicaRepositorio
    {
        void Add(PessoaJuridica pessoaJuridica);
        void Update(PessoaJuridica pessoaJuridica, string cnpj);
        void Delete(string cnpj);
        List<PessoaJuridica> Get(string? cnpj);
    }
}
