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
            Validations.Validations.ValidatePessoaFisica(pessoaFisica);
            _context.PessoasFisicas.Add(pessoaFisica);
            _context.SaveChanges();
        }

        public void Update(PessoaFisica pessoaFisica, string cpf)
        {
            Validations.Validations.ValidatePessoaFisica(pessoaFisica);

            var pessoaExistente = _context.PessoasFisicas.FirstOrDefault(p => p.CPF == cpf);
            if (pessoaExistente != null)
            {
                if (!string.IsNullOrEmpty(pessoaFisica.NomeCompleto))
                {
                    pessoaExistente.NomeCompleto = pessoaFisica.NomeCompleto;
                }

                if (pessoaFisica.DataDeNascimento.HasValue && pessoaFisica.DataDeNascimento.Value.TimeOfDay == TimeSpan.Zero)
                {
                    pessoaFisica.DataDeNascimento = pessoaExistente.DataDeNascimento;
                }
                else
                {
                    pessoaExistente.DataDeNascimento = pessoaFisica.DataDeNascimento;
                }

                if (pessoaFisica.Endereco != "teste")
                {
                    pessoaExistente.Endereco = pessoaFisica.Endereco;
                }

                if (pessoaFisica.Contatos != null && pessoaFisica.Contatos.Count > 0)
                {
                    foreach (var contato in pessoaFisica.Contatos)
                    {
                        var existingContato = pessoaExistente.Contatos!.FirstOrDefault(c => c.Id == contato.Id);
                        if (existingContato != null)
                        {
                            if (contato.Email != "teste@teste.com")
                                existingContato.Email = contato.Email;

                            if (contato.Telefone != "00000000000")
                                existingContato.Telefone = contato.Telefone;
                        }
                        else
                        {
                            pessoaExistente.Contatos!.Add(contato);
                        }
                    }
                }

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
