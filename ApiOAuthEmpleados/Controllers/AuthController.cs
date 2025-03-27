using System.IdentityModel.Tokens.Jwt;
using ApiOAuthEmpleados.Helper;
using ApiOAuthEmpleados.Models;
using ApiOAuthEmpleados.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ApiOAuthEmpleados.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private RepositoryEmpleados repo;

        private HelperActionServicesOAuth helper;

        public AuthController(RepositoryEmpleados repo, HelperActionServicesOAuth helper)
        {
            this.repo = repo;
            this.helper = helper;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult>
        Login(LoginModel model)
        {
            Empleado empleado = await
            this.repo.LoginAsync(model.UserName, int.Parse(model.Password));

            if (empleado != null)
            {
                //DEBEMOS CREAR UNAS CREDENCIALES PARA 
                //INCLUIRLAS DENTRO DEL TOKEN Y QUE ESTARAN 
                //COMPUESTAS POR EL SECRET KEY CIFRADO Y EL 
                //TIPO DE CIFRADO QUE INCLUIREMOS EN EL TOKEN
                SigningCredentials credentials = new SigningCredentials(this.helper.GetKeyToken(), SecurityAlgorithms.HmacSha256);
                //EL TOKEN SE GENERA CON UNA CLASE
                //Y DEBEMOS INDICAR LOS DATOS QUE ALMACENARA EN SU 
                //INTERIOR
                JwtSecurityToken token = new JwtSecurityToken(
                    issuer: this.helper.Issuer,
                    audience: this.helper.Audience,
                    signingCredentials: credentials,
                    expires: DateTime.UtcNow.AddMinutes(20),
                    notBefore: DateTime.UtcNow
                );
                //POR ULTIMO, DEVOLVEMOS LA RESPUESTA AFIRMATIVA
                //CON UN OBJETO QUE CONTENGA EL TOKEN (anonimo)
                return Ok(new
                {
                    response = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }


            return Unauthorized();

        }

    }
}
