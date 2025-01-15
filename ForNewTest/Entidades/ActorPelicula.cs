namespace ForNewTest.Entidades
{
    public class ActorPelicula
    {
        public int ActorModelId { get; set; }
        public int PeliculaModelId { get; set; }
        public ActorModel Actor { get; set; } = null!;
        public PeliculaModel Pelicula { get; set; } = null!;
        public int Orden { get; set; }
        public string Personaje { get; set; } = null!;
    }
}
