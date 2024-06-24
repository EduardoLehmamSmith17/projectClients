using projeto_clientes.Data;
using projeto_clientes.Models;
using projeto_clientes.Repositorio.Interfaces;

namespace projeto_clientes.Repositorio
{
    public class PessoaJuridicaRepositorio : IPessoaJuridicaRepositorio
    {
        private readonly dbContext _context;

        public PessoaJuridicaRepositorio(dbContext dbContext)
        {
            _context = dbContext;
        }

        public void Add(PessoaJuridica pessoaJuridica)
        {
            Validations.Validations.ValidatePessoaJuridica(pessoaJuridica);
            _context.PessoasJuridicas.Add(pessoaJuridica);
            _context.SaveChanges();
        }

        public void Update(PessoaJuridica pessoaJuridica, string cnpj)
        {
            Validations.Validations.ValidatePessoaJuridica(pessoaJuridica);

            var pessoaExistente = _context.PessoasJuridicas.FirstOrDefault(p => p.CNPJ == cnpj);
            if (pessoaExistente != null)
            {
                pessoaExistente.RazaoSocial = pessoaJuridica.RazaoSocial;
                pessoaExistente.NomeFantasia = pessoaJuridica.NomeFantasia;
                pessoaExistente.Endereco = pessoaJuridica.Endereco;

                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentException("Pessoa jurídica não encontrada para o CNPJ especificado.", nameof(cnpj));
            }
        }

        public void Delete(string cnpj)
        {
            var pessoaParaExcluir = _context.PessoasJuridicas.FirstOrDefault(p => p.CNPJ == cnpj);
            if (pessoaParaExcluir != null)
            {
                _context.PessoasJuridicas.Remove(pessoaParaExcluir);
                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentException("Pessoa jurídica não encontrada para o CNPJ especificado.", nameof(cnpj));
            }
        }

        public List<PessoaJuridica> Get(string? cnpj)
        {
            if (string.IsNullOrEmpty(cnpj))
            {
                return _context.PessoasJuridicas.ToList();
            }
            else
            {
                return _context.PessoasJuridicas.Where(p => p.CNPJ == cnpj).ToList();
            }
        }
    }
}
