using projeto_clientes.Models;

namespace projeto_clientes.Repositorio.Interfaces
{
    public interface IPessoaFisicaRepositorio
    {
        void Add(PessoaFisica pessoaFisica);
        void Update(PessoaFisica pessoaFisica, string cpf);
        void Delete(string cpf);
        List<PessoaFisica> Get(string? cpf);
    }
}
