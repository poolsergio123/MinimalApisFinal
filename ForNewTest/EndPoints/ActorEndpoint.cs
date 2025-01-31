using AutoMapper;
using FluentValidation;
using ForNewTest.DTO_s;
using ForNewTest.Entidades;
using ForNewTest.Filtros;
using ForNewTest.IRepositorio;
using ForNewTest.Servicios;
using ForNewTest.Utilidades;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.OpenApi.Models;
using System.ComponentModel.DataAnnotations;

namespace ForNewTest.EndPoints
{
    public static class ActorEndpoint
    {
        private static readonly string contenedor = "actores";
        public static RouteGroupBuilder MapActor(this RouteGroupBuilder group)
        {
            group.MapPost("/", CrearActor).DisableAntiforgery().AddEndpointFilter<FiltroDeValidaciones<CrearActorDTO>>().RequireAuthorization("esadmin").WithOpenApi();
            group.MapGet("/", ObtenerActores).CacheOutput(opt => opt.Expire(TimeSpan.FromSeconds(60)).Tag("actores-get"))
                    .AgregarParametrosAOpenApi();
            group.MapGet("/{id:int}", ObtenerPorId);
            group.MapGet("/{nombre}", ObtenerPorNombre);
            group.MapPut("/{id:int}", ActualizarActor).DisableAntiforgery().AddEndpointFilter<FiltroDeValidaciones<CrearActorDTO>>().RequireAuthorization("esadmin").WithOpenApi();
            group.MapDelete("/{id:int}", EliminarActor).RequireAuthorization("esadmin");
            return group;
        }

        static async Task<Results<Created<ActorDTO>,ValidationProblem>> CrearActor([FromForm] CrearActorDTO crearActorDTO, IActorRepositorio actorRepositorio, IOutputCacheStore outputCacheStore, IMapper mapper,IAlmacenadorArchivos almacenadorArchivos)
        {

            #region Con Ivalidator<CrearActorDTO>
            //var resvalidacion = await validator.ValidateAsync(crearActorDTO);
            //if (!resvalidacion.IsValid)
            //{
            //    return TypedResults.ValidationProblem(resvalidacion.ToDictionary());
            //}
            #endregion
            var actor = mapper.Map<ActorModel>(crearActorDTO);
            if (crearActorDTO.Foto is not null)
            {
                var url = await almacenadorArchivos.Almacenar(contenedor, crearActorDTO.Foto);
                actor.Foto = url;
            }
            var id = await actorRepositorio.CrearActor(actor);
            await outputCacheStore.EvictByTagAsync("actores-get",default);
            var actorDTO = mapper.Map<ActorDTO>(actor);
            return TypedResults.Created($"actores/{id}", actorDTO);
        }

        static async Task<Ok<List<ActorDTO>>> ObtenerPorNombre(string nombre, IActorRepositorio actorRepositorio, IMapper mapper)
        {
            var actores = await actorRepositorio.ObtenerPorNombre(nombre);
            var actoresDTO = mapper.Map<List<ActorDTO>>(actores);
            return TypedResults.Ok(actoresDTO);
        }
        static async Task<Ok<List<ActorDTO>>> ObtenerActores(IActorRepositorio actorRepositorio, IMapper mapper,PaginaDTO paginacion)
        {
            var actores = await actorRepositorio.ObtenerActores(paginacion);
            var actoresDTO = mapper.Map<List<ActorDTO>>(actores);
            return TypedResults.Ok(actoresDTO);
        }

        static async Task<Results<Ok<ActorDTO>,NotFound>> ObtenerPorId(IActorRepositorio actorRepositorio, IMapper mapper,int id)
        {
            if (!await actorRepositorio.Existe(id))
            {
                return TypedResults.NotFound();
            }
            var actor = await actorRepositorio.ObtenerActorPorId(id);

            var actorDTO = mapper.Map<ActorDTO>(actor);
            return TypedResults.Ok(actorDTO);
        }

        static async Task<Results<NoContent,NotFound,ValidationProblem>> ActualizarActor([FromForm]CrearActorDTO crearActorDTO,int id ,IActorRepositorio actorRepositorio, IMapper mapper, IOutputCacheStore outputCacheStore,IAlmacenadorArchivos almacenadorArchivos)
        {

            #region Con IValidator<CrearActorDTO> validator
            //var resvalidacion = await validator.ValidateAsync(crearActorDTO);
            //if (!resvalidacion.IsValid)
            //{
            //    return TypedResults.ValidationProblem(resvalidacion.ToDictionary());
            //}

            #endregion
            var actorDB = await actorRepositorio.ObtenerActorPorId(id);
            if (actorDB is null)
            {
                return TypedResults.NotFound();
            }
            var actor = mapper.Map<ActorModel>(crearActorDTO);
            actor.Id= id;
            actor.Foto = actorDB.Foto;
            if (crearActorDTO.Foto is not null)
            {
                var url = await almacenadorArchivos.Editar(actor.Foto, contenedor, crearActorDTO.Foto);
                actor.Foto = url;
            }
            await actorRepositorio.ActualizarActor(actor);
            await outputCacheStore.EvictByTagAsync("actores-get",default);
            return TypedResults.NoContent();
        }
        static async Task<Results<NoContent, NotFound>> EliminarActor(IActorRepositorio actorRepositorio, int id,IMapper mapper, IOutputCacheStore outputCacheStore, IAlmacenadorArchivos almacenadorArchivos)
        {
            var actorDB = await actorRepositorio.ObtenerActorPorId(id);
            if (actorDB is null)
            {
                return TypedResults.NotFound();
            }
            await almacenadorArchivos.Borrar(actorDB.Foto,contenedor);
            await actorRepositorio.EliminarActor(id);
            await outputCacheStore.EvictByTagAsync("actores-get", default);
            return TypedResults.NoContent();
        }
    }
}
