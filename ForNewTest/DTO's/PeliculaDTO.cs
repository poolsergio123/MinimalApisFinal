﻿using ForNewTest.Entidades;

namespace ForNewTest.DTO_s
{
    public class PeliculaDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public bool EnCines { get; set; }
        public DateTime FechaLanzamiento { get; set; }
        public string Poster { get; set; } = null!;
        public List<ComentarioDTO> Comentarios { get; set; } = new List<ComentarioDTO>();
        public List<GeneroDTO> Generos { get; set; } = new List<GeneroDTO>();
        public List<ActorPeliculaDTO> Actores { get; set; } = new List<ActorPeliculaDTO>();
    }
}
