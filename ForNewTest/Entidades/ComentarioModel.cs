namespace ForNewTest.Entidades
{
    public class ComentarioModel
    {
        public int Id { get; set; }
        public string Cuerpo { get; set; }
        public int PeliculaModelId { get; set; }
    }

    public class Error
    {
        public string error { get; set; }
    }
}
