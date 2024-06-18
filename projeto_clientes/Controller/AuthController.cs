using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using projeto_clientes.Services;

namespace projeto_clientes.Controller
{
    public class AuthController : ControllerBase
    {
        [HttpPost]
        public IActionResult Auth(string username, string password)
        {
            if(username == "pessoaFisica" || password == "12345")
            {
                var token = TokenService.GenerateToken(new Models.PessoaFisica());
                return Ok(token);
            }

            if (username == "pessoaJuridica" || password == "12345")
            {
                var token = TokenService.GenerateToken(new Models.PessoaFisica());
                return Ok(token);
            }

            return BadRequest("username or password invalid");
        }
    }
}
