using AutoMapper;
using ForNewTest.DTO_s;
using ForNewTest.Entidades;
using ForNewTest.Filtros;
using ForNewTest.IRepositorio;
using ForNewTest.Servicios;
using ForNewTest.Utilidades;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace ForNewTest.EndPoints
{
    public static class PeliculaEndPoint
    {
        private static readonly string contenedor = "pelicula";

        public static RouteGroupBuilder MapPelicula(this RouteGroupBuilder group)
        {
            group.MapGet("/",ObtenerPeliculas).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("pelicula-get"))
                .AgregarParametrosAOpenApi();
            group.MapGet("/{id:int}",ObtenerPorId);
            group.MapGet("/{titulo}",ObtenerPorNombre );
            group.MapPost("/",CrearPelicula).DisableAntiforgery().AddEndpointFilter<FiltroDeValidaciones<CrearPeliculaDTO>>().RequireAuthorization("esadmin").WithOpenApi();
            group.MapPut("/{id:int}",ActualizarPelicula).DisableAntiforgery().AddEndpointFilter<FiltroDeValidaciones<CrearPeliculaDTO>>().RequireAuthorization("esadmin").WithOpenApi();
            group.MapDelete("/{id:int}",EliminarPelicula).RequireAuthorization("esadmin");
            group.MapPost("/{id:int}/asignargeneros", AsignarGeneros);
            group.MapPost("/{id:int}/asignaractores",AsignarActores);

            group.MapGet("/filtrar",FiltrarPeliculas).AgregarParametrosPeliculasFiltroAOpenApi();
            return group;
        }
        static async Task<Results<Ok<PeliculaDTO>, NotFound>> ObtenerPorId(int id, IMapper mapper, IPeliculaRepositorio repositorio)
        {
            var peliculaDB = await repositorio.ObtenerPorId(id);
            if (peliculaDB is null)
            {
                return TypedResults.NotFound();
            }

            var peliculaDTO = mapper.Map<PeliculaDTO>(peliculaDB);
            return TypedResults.Ok(peliculaDTO);
        }

        static async Task<Created<PeliculaDTO>> CrearPelicula([FromForm] CrearPeliculaDTO crearPeliculaDTO, IMapper mapper, IPeliculaRepositorio repositorio, IOutputCacheStore outputCache, IAlmacenadorArchivos almacenadorArchivos)
        {
            var Pelicula = mapper.Map<PeliculaModel>(crearPeliculaDTO);
            if (crearPeliculaDTO.Poster is not null)
            {
                var url = await almacenadorArchivos.Almacenar(contenedor, crearPeliculaDTO.Poster);
                Pelicula.Poster = url;
            }
            var id = await repositorio.CrearPelicula(Pelicula);
            await outputCache.EvictByTagAsync("pelicula-get", default);

            var peliculaDTO = mapper.Map<PeliculaDTO>(Pelicula);
            return TypedResults.Created($"/pelicula/{id}", peliculaDTO);
        }

        static async Task<Results<NoContent, NotFound>> ActualizarPelicula([FromForm] CrearPeliculaDTO crearPeliculaDTO, int id, IMapper mapper, IPeliculaRepositorio repositorio, IOutputCacheStore outputCache, IAlmacenadorArchivos almacenadorArchivos)
        {
            var peliculaDB = await repositorio.ObtenerPorId(id);
            if (peliculaDB is null)
            {
                return TypedResults.NotFound();
            }

            var pelicula = mapper.Map<PeliculaModel>(crearPeliculaDTO);
            pelicula.Id = id;
            pelicula.Poster=peliculaDB.Poster;
            if (crearPeliculaDTO.Poster is not null)
            {
                var url = await almacenadorArchivos.Editar(peliculaDB.Poster, contenedor, crearPeliculaDTO.Poster);
                pelicula.Poster = url;
            }
            await repositorio.Actualizar(pelicula);
            await outputCache.EvictByTagAsync("pelicula-get", default);
            return TypedResults.NoContent();

        }
        static async Task<Ok<List<PeliculaDTO>>> ObtenerPeliculas(IMapper mapper, IPeliculaRepositorio peliculaRepositorio,PaginaDTO paginacion)
        {
            var peliculadb = await peliculaRepositorio.ObtenerPeliculas(paginacion);
            var peliculasDTO = mapper.Map<List<PeliculaDTO>>(peliculadb);
            return TypedResults.Ok(peliculasDTO);
        }

        static async Task<Results<NoContent,NotFound>> EliminarPelicula(IPeliculaRepositorio peliculaRepositorio,IOutputCacheStore outputCacheStore, int id, IAlmacenadorArchivos almacenadorArchivos)
        {
            var peliculadb = await peliculaRepositorio.ObtenerPorId(id);
            if (peliculadb is null)
            {
                return TypedResults.NotFound();
            }
            await peliculaRepositorio.EliminarPelicular(id);
            await almacenadorArchivos.Borrar(peliculadb.Poster,contenedor);
            await outputCacheStore.EvictByTagAsync("pelicula-get",default);
            return TypedResults.NoContent();
        }
        static async Task<Ok<List<PeliculaDTO>>> ObtenerPorNombre(IPeliculaRepositorio peliculaRepositorio,IMapper mapper, string titulo)
        {
            var peliculadb = await peliculaRepositorio.ObtenerPorNombre(titulo);
            var peliculasDTO = mapper.Map<List<PeliculaDTO>>(peliculadb);
            return TypedResults.Ok(peliculasDTO);

        }

        static async Task<Results<NoContent,NotFound,BadRequest<string>>> AsignarGeneros(int id, List<int> generosids,IPeliculaRepositorio peliculaRepositorio, IGeneroRepositorio generoRepositorio)
        {
            if (!await peliculaRepositorio.Existe(id))
            {
                return TypedResults.NotFound();
            }

            var generosExistentes= new List<int>();

            if (generosids.Count != 0)
            {
                generosExistentes = await generoRepositorio.Existen(generosids);
            }
            if (generosExistentes.Count !=  generosids.Count)
            {
                var noExistentes = generosids.Except(generosExistentes);
                return TypedResults.BadRequest($"{(noExistentes.Count() >=2 ? "Los":"El")} genero{(noExistentes.Count() >= 2 ? "s" : "")} de id {string.Join(", ",noExistentes)} no existe{(noExistentes.Count() >= 2 ? "n" : "")}.");
            }
            await peliculaRepositorio.AsignarGeneros(id,generosids);
            return TypedResults.NoContent();
        }
        static async Task<Results<NoContent, NotFound, BadRequest<string>>> AsignarActores(int id, List<AsignarActorPeliculaDTO> actoresDTO, IPeliculaRepositorio peliculaRepositorio, IActorRepositorio actorRepositorio, IMapper mapper)
        {
            if (!await peliculaRepositorio.Existe(id))
            {
                return TypedResults.NotFound();
            }
            
            var actoresExistentes= new List<int>();

            var actoresid = actoresDTO.Select(x=>x.ActorModelId).ToList();
            if (actoresDTO.Count !=0)
            {
                actoresExistentes = await actorRepositorio.Existen(actoresid);
            }

            if (actoresExistentes.Count != actoresDTO.Count)
            {
                var noExistentes = actoresid.Except(actoresExistentes);
                return TypedResults.BadRequest($"{(noExistentes.Count() >=2 ? "Los":"El")} actor{(noExistentes.Count() >= 2 ? "es" : "")} de id {string.Join(", ", noExistentes)} no existe{(noExistentes.Count() >= 2 ? "n" : "")}.");
            }

            var actores = mapper.Map<List<ActorPelicula>>(actoresDTO);
            await peliculaRepositorio.AsignarActores(id,actores);
            return TypedResults.NoContent();
        }

        static async Task<Ok<List<PeliculaDTO>>> FiltrarPeliculas(PeliculasFiltrarDTO peliculasFiltrarDTO, [AsParameters] ParametrosFiltrarPeliculasDTO modelo)
        {
            var peliculas = await modelo.Repositorio.Filtrar(peliculasFiltrarDTO);
            var peliculasDTO = modelo.Mapper.Map<List<PeliculaDTO>>(peliculas);
            return TypedResults.Ok(peliculasDTO);
        }

    }
}
