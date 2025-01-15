namespace ForNewTest.Entidades
{
    public class ActorModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Foto { get; set; }
        public List<ActorPelicula> ActoresPeliculas= new List<ActorPelicula>();
    }
}
