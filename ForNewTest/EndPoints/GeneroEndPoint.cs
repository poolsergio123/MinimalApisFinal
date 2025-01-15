using AutoMapper;
using FluentValidation;
using ForNewTest.DTO_s;
using ForNewTest.Entidades;
using ForNewTest.Filtros;
using ForNewTest.IRepositorio;
using ForNewTest.Repositorio;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using System.ComponentModel.DataAnnotations;

namespace ForNewTest.EndPoints
{
    public static class GeneroEndPoint
    {
        public static RouteGroupBuilder MapGenero(this RouteGroupBuilder group) {
            group.MapGet("/", ListarGenero).CacheOutput(exp => exp.Expire(TimeSpan.FromSeconds(60)).Tag("genero-cache")).RequireAuthorization();
            group.MapGet("/{id}", ObtenerGeneroPorId);
            group.MapPost("generos/", CrearGenero).AddEndpointFilter<FiltroDeValidaciones<CrearGeneroDTO>>();
            group.MapPut("generos/{id}", ActualizarGenero).AddEndpointFilter<FiltroDeValidaciones<CrearGeneroDTO>>();
            group.MapDelete("delete/{id}", EliminarGenero);
            return group;
        }


        static async Task<Ok<List<GeneroDTO>>> ListarGenero(IGeneroRepositorio generoRepositorio,IMapper mapper) {
            var generos = await generoRepositorio.ObtenerGeneros();
            var generosDTO = mapper.Map<List<GeneroDTO>>(generos);
            return  TypedResults.Ok(generosDTO);
        }
        

        static async Task<Results<Ok<GeneroDTO>, NotFound>> ObtenerGeneroPorId(int id, IGeneroRepositorio generoRepositorio,IMapper mapper)
        {
            var genero = await generoRepositorio.ObtenerGeneroPorId(id);
            var generoDTO = mapper.Map<GeneroDTO>(genero);
            if (!await generoRepositorio.Existe(id))
            {
                return TypedResults.NotFound();
            }
            return TypedResults.Ok(generoDTO);
        }

        static async Task<Results<Created<GeneroDTO>,ValidationProblem>> CrearGenero(CrearGeneroDTO creargeneroDTO, IGeneroRepositorio generoRepositorio, IOutputCacheStore outputCacheStore,IMapper mapper)
        {
            #region Con Ivalidator<CrearGeneroDTO>
            //var resultadoValidacion = await validator.ValidateAsync(creargeneroDTO);

            //if (!resultadoValidacion.IsValid)
            //{
            //    return TypedResults.ValidationProblem(resultadoValidacion.ToDictionary());
            //}
            #endregion

            var genero =mapper.Map<GeneroModel>(creargeneroDTO);
            var id = await generoRepositorio.CrearGenero(genero);

            var generoDTO = mapper.Map<GeneroDTO>(genero);
            await outputCacheStore.EvictByTagAsync("genero-cache", default);
            return TypedResults.Created($"/generos/{id}", generoDTO);
        }
        static async Task<Results<NoContent, NotFound,ValidationProblem>> ActualizarGenero(int id, CrearGeneroDTO creargeneroDTO, IGeneroRepositorio generoRepositorio, IOutputCacheStore outputCacheStore,IMapper mapper)
        {
            #region Con Ivalidator<CrearGeneroDTO>
            //var resultadoValidacion = await validator.ValidateAsync(creargeneroDTO);

            //if (!resultadoValidacion.IsValid)
            //{
            //    return TypedResults.ValidationProblem(resultadoValidacion.ToDictionary());
            //}
            #endregion
            if (!await generoRepositorio.Existe(id))
            {
                return TypedResults.NotFound();
            }
            var genero = mapper.Map<GeneroModel>(creargeneroDTO);
            genero.Id = id;
            await generoRepositorio.ActualizarGenero(genero);
            await outputCacheStore.EvictByTagAsync("genero-cache", default);

            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound>> EliminarGenero(int id, IGeneroRepositorio generoRepositorio, IOutputCacheStore outputCacheStore)
        {
            if (!await generoRepositorio.Existe(id))
            {
                return TypedResults.NotFound();
            }
            await generoRepositorio.EliminarGenero(id);
            await outputCacheStore.EvictByTagAsync("genero-cache", default);

            return TypedResults.NoContent();
        }
    }
}
