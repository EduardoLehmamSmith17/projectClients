using projeto_clientes.Data;
using projeto_clientes.Models;
using projeto_clientes.Repositorio.Interfaces;

namespace projeto_clientes.Repositorio
{
    public class PessoaFisicaRepositorio : IPessoaFisicaRepositorio
    {
        private readonly dbContext _context;

        public PessoaFisicaRepositorio(dbContext dbContext)
        {
            _context = dbContext;
        }

        public void Add(PessoaFisica pessoaFisica)
        {
            _context.PessoasFisicas.Add(pessoaFisica);
            _context.SaveChanges();
        }

        public void Update(PessoaFisica pessoaFisica, string cpf)
        {
            var pessoaExistente = _context.PessoasFisicas.FirstOrDefault(p => p.CPF == cpf);
            if (pessoaExistente != null)
            {
                pessoaExistente.NomeCompleto = pessoaFisica.NomeCompleto;
                pessoaExistente.DataDeNascimento = pessoaFisica.DataDeNascimento;
                pessoaExistente.Endereco = pessoaFisica.Endereco;

                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentException("Pessoa física não encontrada para o CPF especificado.", nameof(cpf));
            }
        }

        public void Delete(string cpf)
        {
            var pessoaParaExcluir = _context.PessoasFisicas.FirstOrDefault(p => p.CPF == cpf);
            if (pessoaParaExcluir != null)
            {
                _context.PessoasFisicas.Remove(pessoaParaExcluir);
                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentException("Pessoa física não encontrada para o CPF especificado.", nameof(cpf));
            }
        }

        public List<PessoaFisica> Get(string? cpf)
        {
            if (string.IsNullOrEmpty(cpf))
            {
                return _context.PessoasFisicas.ToList();
            }
            else
            {
                return _context.PessoasFisicas.Where(p => p.CPF == cpf).ToList();
            }
        }
    }
}
