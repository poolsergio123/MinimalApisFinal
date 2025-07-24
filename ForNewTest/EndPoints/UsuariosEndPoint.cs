using FluentValidation;
using ForNewTest.DTO_s;
using ForNewTest.Filtros;
using ForNewTest.Servicios;
using ForNewTest.Utilidades;
using ForNewTest.Validaciones;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Xml.Serialization;

namespace ForNewTest.EndPoints
{
    public static class UsuariosEndPoint
    {
        public static RouteGroupBuilder MapUsuarios(this RouteGroupBuilder group)
        {
            group.MapPost("/registrar",Registrar).AddEndpointFilter<FiltroDeValidaciones<CredencialesUsuarioDTO>>();
            group.MapPost("/login", Login).AddEndpointFilter<FiltroDeValidaciones<CredencialesUsuarioDTO>>();
            group.MapPost("/haceradmin", HacerAdmin).AddEndpointFilter<FiltroDeValidaciones<EditarClaimDTO>>();
            //.RequireAuthorization("esadmin");
            group.MapPost("/remover", RemoverAdmin).AddEndpointFilter<FiltroDeValidaciones<EditarClaimDTO>>()
                .RequireAuthorization("esadmin");
            group.MapGet("/renovarToken",RenovarToken).RequireAuthorization();
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
                var respuestaAutenticacion = await ConstruirToken(credencialesUsuarioDTO,configuration, userManager);
                return TypedResults.Ok(respuestaAutenticacion);
            }
            else
            {
                return TypedResults.BadRequest("Login Incorrecto.");
            }
        
        }

        static async Task<Results<Ok<RespuestaAutenticacionDTO>, NotFound>> RenovarToken(IServiciosUsuarios serviciosUsuarios, IConfiguration configuration, [FromServices]UserManager<IdentityUser> userManager)
        {
            var usuario = await serviciosUsuarios.ObtenerUsuario();
            if (usuario == null) { return TypedResults.NotFound(); }
            var creadencialesUsuarioDTO = new CredencialesUsuarioDTO { Email=usuario.Email!};
            var respuestaAutenticacionDTO = await ConstruirToken( creadencialesUsuarioDTO,configuration,userManager);
            return TypedResults.Ok( respuestaAutenticacionDTO);
        }

        static async Task<Results<NotFound,NoContent>> HacerAdmin(EditarClaimDTO editarClaimDTO, [FromServices] UserManager<IdentityUser> userManager)
        {
            var usuario = await userManager.FindByEmailAsync(editarClaimDTO.Email);

            if (usuario is null)
            {
                return TypedResults.NotFound();
            }

            await userManager.AddClaimAsync(usuario, new Claim("esadmin","true"));
            return TypedResults.NoContent();
        }

        static async Task<Results<NotFound, NoContent>> RemoverAdmin(EditarClaimDTO editarClaimDTO, [FromServices] UserManager<IdentityUser> userManager)
        {
            var usuario = await userManager.FindByEmailAsync(editarClaimDTO.Email);

            if (usuario is null)
            {
                return TypedResults.NotFound();
            }

            await userManager.RemoveClaimAsync(usuario, new Claim("esadmin", "true"));
            return TypedResults.NoContent();
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
                var credencialesRespuesta = await ConstruirToken(credencialesUsuarioDTO,configuration,userManager);
                return TypedResults.Ok(credencialesRespuesta);
            }
            else
            {
                return TypedResults.BadRequest(resultado.Errors);
            }
        }
        private async static Task<RespuestaAutenticacionDTO> ConstruirToken(CredencialesUsuarioDTO credencialesUsuarioDTO,IConfiguration configuration, UserManager<IdentityUser> userManager)
        {
            var claims = new List<Claim> {
                new Claim("email",credencialesUsuarioDTO.Email),
                new Claim("aadasd","asdasdas")
            };

            var usuario = await userManager.FindByNameAsync(credencialesUsuarioDTO.Email);
            var claimsDB = await userManager.GetClaimsAsync(usuario!);

            claims.AddRange(claimsDB);

            var llave = Llaves.ObtenerLlave(configuration);
            var creds = new SigningCredentials(llave.First(),SecurityAlgorithms.HmacSha256);
            var expiracion = DateTime.UtcNow.AddHours(1);

            var tokenDeSeguridad = new  JwtSecurityToken(issuer:null,audience:null,claims:claims,expires:expiracion,signingCredentials: creds);

            var token = new JwtSecurityTokenHandler().WriteToken(tokenDeSeguridad);

            return new RespuestaAutenticacionDTO
            {
                Token = token,
                Expiracion = expiracion
            };
        }
    }
}
