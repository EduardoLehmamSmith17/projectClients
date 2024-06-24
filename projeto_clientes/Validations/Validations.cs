using projeto_clientes.Models;

namespace projeto_clientes.Validations;

public static class Validations
{
    public static void ValidatePessoaFisica(PessoaFisica pessoaFisica)
    {
        if (string.IsNullOrEmpty(pessoaFisica.NomeCompleto) || pessoaFisica.NomeCompleto.Length > 200)
        {
            throw new ArgumentException("NomeCompleto é obrigatório e deve ter no máximo 200 caracteres.");
        }

        if (string.IsNullOrEmpty(pessoaFisica.CPF) || pessoaFisica.CPF.Length != 11)
        {
            throw new ArgumentException("CPF é obrigatório, deve ter 11 caracteres e ser um CPF válido.");
        }

        if (pessoaFisica.DataDeNascimento == null || pessoaFisica.DataDeNascimento.Value > DateTime.Now)
        {
            throw new ArgumentException("DataDeNascimento deve ser uma data válida e anterior à data atual.");
        }

        if (pessoaFisica.Contatos == null || !pessoaFisica.Contatos.Any())
        {
            throw new ArgumentException("Contatos são obrigatórios.");
        }

        foreach (var contato in pessoaFisica.Contatos)
        {
            ValidateContato(contato);
        }
    }

    public static void ValidatePessoaJuridica(PessoaJuridica pessoaJuridica)
    {
        if (pessoaJuridica == null)
        {
            throw new ArgumentNullException(nameof(pessoaJuridica), "Pessoa Jurídica não pode ser nula.");
        }

        if (string.IsNullOrWhiteSpace(pessoaJuridica.RazaoSocial))
        {
            throw new ArgumentException("Razão Social é obrigatória.", nameof(pessoaJuridica.RazaoSocial));
        }

        if (string.IsNullOrWhiteSpace(pessoaJuridica.CNPJ) || pessoaJuridica.CNPJ.Length != 14)
        {
            throw new ArgumentException("CNPJ é obrigatório e deve ter 14 caracteres.", nameof(pessoaJuridica.CNPJ));
        }

        if (pessoaJuridica.RazaoSocial.Length > 200)
        {
            throw new ArgumentException("Razão Social não pode ter mais de 200 caracteres.", nameof(pessoaJuridica.RazaoSocial));
        }

        if (!string.IsNullOrEmpty(pessoaJuridica.NomeFantasia) && pessoaJuridica.NomeFantasia.Length > 200)
        {
            throw new ArgumentException("Nome Fantasia não pode ter mais de 200 caracteres.", nameof(pessoaJuridica.NomeFantasia));
        }

        if (!string.IsNullOrEmpty(pessoaJuridica.Endereco) && pessoaJuridica.Endereco.Length > 200)
        {
            throw new ArgumentException("Endereço não pode ter mais de 200 caracteres.", nameof(pessoaJuridica.Endereco));
        }

        if (pessoaJuridica.Contatos == null || !pessoaJuridica.Contatos.Any())
        {
            throw new ArgumentException("Contatos são obrigatórios.", nameof(pessoaJuridica.Contatos));
        }

        foreach (var contato in pessoaJuridica.Contatos)
        {
            ValidateContato(contato);
        }
    }

    public static void ValidateContato(Contato contato)
    {
        if (string.IsNullOrEmpty(contato.Email) || !IsValidEmail(contato.Email))
        {
            throw new ArgumentException("Email é obrigatório e deve ser um email válido.");
        }

        if (string.IsNullOrEmpty(contato.Telefone))
        {
            throw new ArgumentException("Telefone é obrigatório.");
        }
    }

    public static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}
