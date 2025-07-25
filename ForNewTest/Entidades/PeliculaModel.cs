﻿namespace ForNewTest.Entidades
{
    public class PeliculaModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public bool EnCines { get; set; } 
        public DateTime FechaLanzamiento { get; set; }
        public string? Poster { get; set; } = null!;
        public List<ComentarioModel> Comentarios { get; set; } = new List<ComentarioModel>();
        public List<GeneroPelicula> GeneroPeliculas { get; set; } = new List<GeneroPelicula>();
        public List<ActorPelicula> ActoresPeliculas { get; set; } = new List<ActorPelicula>();

    }
}
