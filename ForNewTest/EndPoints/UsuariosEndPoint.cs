using FluentValidation;
using ForNewTest.DTO_s;
using ForNewTest.Filtros;
using ForNewTest.Utilidades;
using ForNewTest.Validaciones;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ForNewTest.EndPoints
{
    public static class UsuariosEndPoint
    {
        public static RouteGroupBuilder MapUsuarios(this RouteGroupBuilder group)
        {
            group.MapPost("/registrar",Registrar).AddEndpointFilter<FiltroDeValidaciones<CredencialesUsuarioDTO>>();
            group.MapPost("/login", Login).AddEndpointFilter<FiltroDeValidaciones<CredencialesUsuarioDTO>>();
            return group;
        }

        static async Task<Results<Ok<RespuestaAutenticacionDTO>, BadRequest<string>>> Login(CredencialesUsuarioDTO credencialesUsuarioDTO, [FromServices] SignInManager<IdentityUser> signInManager, [FromServices] UserManager<IdentityUser> userManager,IConfiguration configuration )
        {
            var usuario = await userManager.FindByEmailAsync(credencialesUsuarioDTO.Email);

            if (usuario == null) {
                return TypedResults.BadRequest("Login Incorrecto.");
            }

            var resultado = await signInManager.CheckPasswordSignInAsync(usuario, credencialesUsuarioDTO.Password, lockoutOnFailure: false);
            if (resultado.Succeeded)
            {
                var respuestaAutenticacion = ConstruirToken(credencialesUsuarioDTO,configuration);
                return TypedResults.Ok(respuestaAutenticacion);
            }
            else
            {
                return TypedResults.BadRequest("Login Incorrecto.");
            }
        
        }

        static async Task<Results<Ok<RespuestaAutenticacionDTO>,BadRequest<IEnumerable<IdentityError>>>> Registrar(CredencialesUsuarioDTO credencialesUsuarioDTO, [FromServices] UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            var usuario = new IdentityUser
            {
                UserName = credencialesUsuarioDTO.Email,
                Email = credencialesUsuarioDTO.Email
            };

            var resultado = await userManager.CreateAsync(usuario,credencialesUsuarioDTO.Password);
            if (resultado.Succeeded)
            {
                var credencialesRespuesta = ConstruirToken(credencialesUsuarioDTO,configuration);
                return TypedResults.Ok(credencialesRespuesta);
            }
            else
            {
                return TypedResults.BadRequest(resultado.Errors);
            }
        }
        private static RespuestaAutenticacionDTO ConstruirToken(CredencialesUsuarioDTO credencialesUsuarioDTO,IConfiguration configuration)
        {
            var claims = new List<Claim> {
                new Claim("email",credencialesUsuarioDTO.Email),
                new Claim("aadasd","asdasdas")
            };

            var llave = Llaves.ObtenerLlave(configuration);
            var creds = new SigningCredentials(llave.First(),SecurityAlgorithms.HmacSha256);
            var expiracion = DateTime.UtcNow.AddHours(1);

            var tokenDeSeguridad = new JwtSecurityToken(issuer:null,audience:null,claims:claims,expires:expiracion,signingCredentials: creds);

            var token = new JwtSecurityTokenHandler().WriteToken(tokenDeSeguridad);

            return new RespuestaAutenticacionDTO
            {
                Token = token,
                Expiracion = expiracion
                
            };
        }
    }
}
