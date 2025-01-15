using ForNewTest.Migrations;

namespace ForNewTest.Entidades
{
    public class GeneroPelicula
    {
        public int PeliculaModelId { get; set; }
        public int GeneroModelId { get; set; }
        public GeneroModel Genero { get; set; } = null!;
        public PeliculaModel Pelicula { get; set; } = null!;
    }
}
