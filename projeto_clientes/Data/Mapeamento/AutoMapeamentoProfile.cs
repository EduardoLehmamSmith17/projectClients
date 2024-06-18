using AutoMapper;
using projeto_clientes.Models;
using projeto_clientes.ViewModels;

namespace projeto_clientes.Data.Mapeamento
{
    public class AutoMapeamentoProfile : Profile
    {
        public AutoMapeamentoProfile()
        {
            CreateMap<PessoaFisicaViewModel, PessoaFisica>();
            CreateMap<PessoaJuridicaViewModel, PessoaJuridica>();
        }
    }
}
