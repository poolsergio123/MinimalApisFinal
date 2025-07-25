using AutoMapper;
using ForNewTest.DTO_s;
using ForNewTest.Filtros;
using ForNewTest.IRepositorio;
using ForNewTest.Repositorio;
using ForNewTest.Utilidades;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ForNewTest.EndPoints
{
    public static class PersonaEndPoint
    {
        public static RouteGroupBuilder MapPersona(this RouteGroupBuilder group)
        {
            //group.MapPost("/", CrearActor);
            group.MapGet("/", ObtenerPersonas).CacheOutput(opt => opt.Expire(TimeSpan.FromSeconds(60)).Tag("personas-get")).AgregarParametrosAOpenApi();
            //group.MapGet("/{id:int}", ObtenerPorId);
            //group.MapGet("/{nombre}", ObtenerPorNombre);
            //group.MapPut("/{id:int}", ActualizarActor);
            //group.MapDelete("/{id:int}", EliminarActor);
            return group;
        }

        static async Task<Ok<List<PersonaDTO>>> ObtenerPersonas(IPersonaRepositorio personaRepositorio, IMapper mapper, PaginaDTO paginacion)
        {
            var personas = await personaRepositorio.ObtenerPersonas(paginacion);
            var personasDTO = mapper.Map<List<PersonaDTO>>(personas);
            return TypedResults.Ok(personasDTO);
        }
    }
}
