 using AutoMapper;
using ForNewTest.DTO_s;
using ForNewTest.Entidades;

namespace ForNewTest.Utilidades
{
    public class Mapper :Profile
    {
        public Mapper() {
            CreateMap<GeneroModel, GeneroDTO>();
            //CreateMap<List<GeneroModel>, List<GeneroDTO>>();
            CreateMap<CrearGeneroDTO, GeneroModel>();
            CreateMap<ActualizarGeneroDTO, GeneroModel>();


            CreateMap<ActorModel, ActorDTO>();
            //CreateMap<List<ActorModel>, List<ActorDTO>>();
            CreateMap<CrearActorDTO, ActorModel>().ForMember(x => x.Foto, opciones => opciones.Ignore());

            CreateMap<PeliculaModel, PeliculaDTO>()
                .ForMember(p => p.Generos, entidad => entidad
                .MapFrom(p => p.GeneroPeliculas.Select(gp => new GeneroDTO { Id = gp.GeneroModelId, Nombre = gp.Genero.Nombre })))
                .ForMember(a => a.Actores, entidad => entidad
                .MapFrom(a => a.ActoresPeliculas.Select(ap => new ActorPeliculaDTO { Id = ap.ActorModelId, Nombre = ap.Actor.Nombre, Personaje = ap.Personaje})));


            //CreateMap<List<ActorModel>, List<ActorDTO>>();
            CreateMap<CrearPeliculaDTO, PeliculaModel>().ForMember(x => x.Poster, opciones => opciones.Ignore());


            CreateMap<ComentarioModel, ComentarioDTO>();
            CreateMap<CrearComentarioDTO, ComentarioModel>();

            CreateMap<AsignarActorPeliculaDTO, ActorPelicula>();

            CreateMap<PersonaModel,PersonaDTO>();
        }
    }
}
