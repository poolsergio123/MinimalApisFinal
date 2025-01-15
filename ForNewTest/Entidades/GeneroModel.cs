namespace ForNewTest.Entidades
{
    public class GeneroModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<GeneroPelicula> GeneroPeliculas { get; set; } = new List<GeneroPelicula>();
    }
}
