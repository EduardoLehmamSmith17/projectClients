using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using projeto_clientes.Models;
using projeto_clientes.Repositorio.Interfaces;
using projeto_clientes.ViewModels;

namespace projeto_clientes.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PessoasController : ControllerBase
    {
        private readonly IPessoaFisicaRepositorio _pessoaFisicaRepositorio;
        private readonly IPessoaJuridicaRepositorio _pessoaJuridicaRepositorio;
        private readonly IMapper _mapper;

        public PessoasController(IPessoaFisicaRepositorio pessoaFisicaRepositorio, IPessoaJuridicaRepositorio pessoaJuridiacRepositorio, IMapper mapper)
        {
            _pessoaFisicaRepositorio = pessoaFisicaRepositorio;
            _pessoaJuridicaRepositorio = pessoaJuridiacRepositorio;
            _mapper = mapper;
        }

        //[Authorize]
        [HttpPost]
        [Route("add-pessoa-fisica")]
        public ActionResult AddPessoaFisica([FromBody] PessoaFisicaViewModel pessoaFisicaViewModel) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pessoaFisica = _mapper.Map<PessoaFisicaViewModel, PessoaFisica>(pessoaFisicaViewModel);

            try
            {
                _pessoaFisicaRepositorio.Add(pessoaFisica);
                return Ok("Pessoa física cadastrada com sucesso.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao cadastrar pessoa física: " + ex.Message);
            }
        }

        //[Authorize]
        [HttpPost]
        [Route("add-pessoa-juridica")]
        public ActionResult AddPessoaJuridica([FromBody] PessoaJuridicaViewModel pessoaJuridicaViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pessoaFisica = _mapper.Map<PessoaJuridicaViewModel, PessoaJuridica>(pessoaJuridicaViewModel);

            try
            {
                _pessoaJuridicaRepositorio.Add(pessoaFisica);
                return Ok("Pessoa jurídica cadastrada com sucesso.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao cadastrar pessoa jurídica: " + ex.Message);
            }
        }

        //[Authorize]
        [HttpPut]
        [Route("update-pessoa-fisica/{cpf}")]
        public ActionResult UpdatePessoaFisica([FromBody] PessoaFisicaViewModel pessoaFisicaViewModel, string cpf)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pessoaFisica = _mapper.Map<PessoaFisicaViewModel, PessoaFisica>(pessoaFisicaViewModel);

            try
            {
                _pessoaFisicaRepositorio.Update(pessoaFisica, cpf);
                return Ok("Pessoa física atualizada com sucesso.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao atualizar pessoa física: " + ex.Message);
            }
        }

        //[Authorize]
        [HttpPut]
        [Route("update-pessoa-juridica/{cnpj}")]
        public ActionResult UpdatePessoaJuridica([FromBody] PessoaJuridicaViewModel pessoaJuridicaViewModel, string cnpj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pessoaJuridica = _mapper.Map<PessoaJuridicaViewModel, PessoaJuridica>(pessoaJuridicaViewModel);

            try
            {
                _pessoaJuridicaRepositorio.Update(pessoaJuridica, cnpj);
                return Ok("Pessoa física atualizada com sucesso.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao atualizar pessoa física: " + ex.Message);
            }
        }

        //[Authorize]
        [HttpGet]
        [Route("get-pessoa-fisica/{cpf}")]
        public ActionResult GetPessoaFisica(string cpf = null)
        {
            try
            {
                if (cpf == null)
                {
                    var pessoasFisicas = _pessoaFisicaRepositorio.Get(cpf);
                    return Ok(pessoasFisicas);
                }
                else
                {
                    var pessoaFisica = _pessoaFisicaRepositorio.Get(cpf);
                    if (pessoaFisica == null || pessoaFisica.Count() == 0)
                    {
                        return NotFound("Pessoa física não encontrada.");
                    }
                    return Ok(pessoaFisica);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao buscar pessoa física: " + ex.Message);
            }
        }

        //[Authorize]
        [HttpGet]
        [Route("get-pessoa-juridica/{cnpj}")]
        public ActionResult GetPessoaJuridica(string cnpj = null)
        {
            try
            {
                if (cnpj == null)
                {
                    var pessoasJuridicas = _pessoaJuridicaRepositorio.Get(cnpj);
                    return Ok(pessoasJuridicas);
                }
                else
                {
                    var pessoaJuridica = _pessoaJuridicaRepositorio.Get(cnpj);
                    if (pessoaJuridica == null || pessoaJuridica.Count() == 0)
                    {
                        return NotFound("Pessoa jurídica não encontrada.");
                    }
                    return Ok(pessoaJuridica);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao buscar pessoa jurídica: " + ex.Message);
            }
        }

        //[Authorize]
        [HttpGet]
        [Route("get-pessoas")]
        public ActionResult GetFisicaPessoa()
        {
            try
            {
                string cpf = null;
                var pessoasFisicas = _pessoaFisicaRepositorio.Get(cpf);
                return Ok(pessoasFisicas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao buscar pessoa física: " + ex.Message);
            }
        }

        //[Authorize]
        [HttpGet]
        [Route("get-pessoa")]
        public ActionResult GetJuridicaPessoa()
        {
            try
            {
                string cnpj = null;
                var pessoasJuridicas = _pessoaJuridicaRepositorio.Get(cnpj);
                return Ok(pessoasJuridicas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao buscar pessoa física: " + ex.Message);
            }
        }

        //[Authorize]
        [HttpDelete]
        [Route("delete-pessoa-fisica/{cpf}")]
        public ActionResult DeletePessoaFisica(string cpf = null)
        {
            try
            {
                if (string.IsNullOrEmpty(cpf))
                {
                    return BadRequest("O CPF não pode ser nulo ou vazio.");
                }

                _pessoaFisicaRepositorio.Delete(cpf);

                return Ok("Pessoa física excluída com sucesso.");
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao excluir pessoa física: {ex.Message}");
            }
        }
        
        //[Authorize]
        [HttpDelete]
        [Route("delete-pessoa-juridica/{cnpj}")]
        public ActionResult DeletePessoaJuridica(string cnpj = null)
        {
            try
            {
                if (string.IsNullOrEmpty(cnpj))
                {
                    return BadRequest("O CNPJ não pode ser nulo ou vazio.");
                }

                _pessoaJuridicaRepositorio.Delete(cnpj);

                return Ok("Pessoa jurídica excluída com sucesso.");
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao excluir pessoa jurídica: {ex.Message}");
            }
        }
    }
}
