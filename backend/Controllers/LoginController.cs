using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace backend.Context
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private readonly MeuDbContext _context;
        private readonly IConfiguration _configuration;

    


        public LoginController(MeuDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        //       5-  Os entrypoints da aplicação devem usar a porta 5000 e ser:
        //(POST) http://0.0.0.0:5000/login/

        [HttpPost()]
        [AllowAnonymous]
        public async Task<ActionResult<string>> Login()
        {

            //3- O login e senha fornecidos devem estar em variáveis de ambiente e
            //    terem uma versão para o ambiente de desenvolvimento vinda de um arquivo
            //    .env no node ou de um arquivo de configuração no ASP.NET.

            string login = _configuration["environments:login"];
            string senha = _configuration["environments:senha"];
           
            Usuarios usuario = _context.usuarios.Where(x=>x.login==login && x.senha==senha).FirstOrDefault();

            if (usuario!=null)
            {

                //1- O sistema deve ter um mecanismo de login usando JWT,
                //com um entrypoint que recebe { "login":"letscode", "senha":"lets@123"}
                //e gera um token.

                return await GerarJwt(usuario);
            }
            else
            {
                return BadRequest();
            }
           
        }



        private async Task<string> GerarJwt(Usuarios user)
        {
                        var tokenHandler = new JwtSecurityTokenHandler();

            //3- O mesmo vale para qualquer "segredo" do sistema, como a chave do JWT.
            var key = Encoding.ASCII.GetBytes(_configuration["environments:JwtKey"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.login.ToString()),
                   
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            string t= ((JwtSecurityToken)token).RawData;


            return t;
        }

    }
}
