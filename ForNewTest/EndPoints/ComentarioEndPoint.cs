using AutoMapper;
using ForNewTest.DTO_s;
using ForNewTest.Entidades;
using ForNewTest.Filtros;
using ForNewTest.IRepositorio;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace ForNewTest.EndPoints
{
    public static class ComentarioEndPoint
    {
        public static RouteGroupBuilder MapComentario(this RouteGroupBuilder group)
        {
            group.MapGet("/", ObtenerComentarios).CacheOutput(c=>c.Expire(TimeSpan.FromSeconds(60)).Tag("comentario-get"));
            group.MapGet("/{id:int}", ObtenerPorId);
            group.MapGet("/{cuerpo}", ObtenerPorNombre);
            group.MapPost("/", CrearComentario).AddEndpointFilter<FiltroDeValidaciones<CrearComentarioDTO>>();
            group.MapPut("/{id:int}", ActualizarComentario).AddEndpointFilter<FiltroDeValidaciones<CrearComentarioDTO>>();
            group.MapDelete("/{id:int}", EliminarComentario);
            return group;
        }
        static async Task<Results<Ok<List<ComentarioDTO>>,NotFound<Error>>> ObtenerPorNombre(string cuerpo,int peliculaid, IComentarioRepositorio comentarioRepositorio, IPeliculaRepositorio peliculaRepositorio, IMapper mapper)
        {
            if (!await peliculaRepositorio.Existe(peliculaid))
            {
                var error = new Error { error ="El id proporcionado de la pelicula no existe."};
                return TypedResults.NotFound(error);
            }
            var comentarios = await comentarioRepositorio.ObtenerPorCuerpo(cuerpo,peliculaid);
            var comentariosDTO = mapper.Map<List<ComentarioDTO>>(comentarios);
            return TypedResults.Ok(comentariosDTO);
        }
        static async Task<Results<Ok<List<ComentarioDTO>>,NotFound<Error>>> ObtenerComentarios(IMapper mapper, IComentarioRepositorio comentarioRepositorio,IPeliculaRepositorio peliculaRepositorio,int peliculaid,int pagina =1,int registrosPorPagina = 10)
        {
            if (!await peliculaRepositorio.Existe(peliculaid))
            {
                var mensaje = new Error { error = "No existe la pelicula especificada." };
                return TypedResults.NotFound(mensaje);
            }
            var paginaDTO = new PaginaDTO { Pagina = pagina,RegistrosPorPagina=registrosPorPagina };
            var comentariosDB = await comentarioRepositorio.ObtenerComentarios(peliculaid,paginaDTO);
            
            var comentariosDTO = mapper.Map<List<ComentarioDTO>>(comentariosDB);
            return TypedResults.Ok(comentariosDTO);
        }

        static async Task<Results<Created<ComentarioDTO>,NotFound<Error>>> CrearComentario(int peliculaid,CrearComentarioDTO crearComentarioDTO,IComentarioRepositorio comentarioRepositorio, IPeliculaRepositorio peliculaRepositorio, IOutputCacheStore outputCache, IMapper mapper)
        {
            if (!await peliculaRepositorio.Existe(peliculaid))
            {
                var error = new Error { error = "No existe la pelicula especificada"};
                return TypedResults.NotFound(error);
            }
            var comentario = mapper.Map<ComentarioModel>(crearComentarioDTO);

            comentario.PeliculaModelId = peliculaid;
            var id = await comentarioRepositorio.CrearComentario(comentario);
            comentario.Id= id;
            var comentarioDTO = mapper.Map<ComentarioDTO>(comentario);
            await outputCache.EvictByTagAsync("comentario-get",default);
            return TypedResults.Created($"/comentario/{id}",comentarioDTO);
        }

        static async Task<Results<Ok<ComentarioDTO>, NotFound<Error>>> ObtenerPorId(int id, IComentarioRepositorio comentarioRepositorio, IPeliculaRepositorio peliculaRepositorio, IMapper mapper, int peliculaid = 1)
        {
            if (!await peliculaRepositorio.Existe(peliculaid))
            {
                var error = new Error { error = "No existe la pelicula con el id proporcionado." };
                return TypedResults.NotFound(error);
            }

            var comentario = await comentarioRepositorio.ObtenerPorId(id,peliculaid);
            if (comentario is null)
            {
                var error = new Error { error = "No existe el comentario con el id proporcionado en la pelicula seleccionada." };
                return TypedResults.NotFound(error);
            }
            var comentarioDTO = mapper.Map<ComentarioDTO> (comentario);
            return TypedResults.Ok(comentarioDTO);
        }
        static async Task<Results<NoContent,NotFound<Error>>> ActualizarComentario(int peliculaid,int id,CrearComentarioDTO crearComentarioDTO, IComentarioRepositorio comentarioRepositorio, IPeliculaRepositorio peliculaRepositorio, IOutputCacheStore outputCache, IMapper mapper)
        {
            var peliculadb = await peliculaRepositorio.ObtenerPorId(peliculaid);
            if (peliculadb is null)
            {
                var error = new Error { error="La pelicula con el id especificado no existe."};
                return TypedResults.NotFound(error);
            }
            var comentariodb = await comentarioRepositorio.ObtenerPorId(id,peliculaid);
            if (comentariodb is null)
            {
                var error = new Error { error = "El comentario con el id especificado no existe en la pelicula indicada." };
                return TypedResults.NotFound(error);
            }
            var comentario = mapper.Map<ComentarioModel>(crearComentarioDTO);
            comentario.PeliculaModelId= peliculaid;
            comentario.Id= id;
            await comentarioRepositorio.ActualizarComentario(comentario);
            await outputCache.EvictByTagAsync("comentario-get",default);
            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound<Error>>> EliminarComentario(int id,IPeliculaRepositorio peliculaRepositorio, IComentarioRepositorio comentarioRepositorio, IOutputCacheStore outputCache, int peliculaid=1)
        {
            if (!await peliculaRepositorio.Existe(peliculaid))
            {
                var error = new Error { error = "El id de la pelicula no existe." };
                return TypedResults.NotFound(error);
            }

            var comentariodb = await comentarioRepositorio.ObtenerPorId(id, peliculaid);

            if (comentariodb is null)
            {
                var error = new Error { error = "El comentario con el id especificado no existe en la pelicula indicada." };
                return TypedResults.NotFound(error);
            }


            await comentarioRepositorio.EliminarComentario(id);
            await outputCache.EvictByTagAsync("comentario-get",default);
            return TypedResults.NoContent();
        }
    }
}
