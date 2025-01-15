namespace ForNewTest.DTO_s
{
    public class CrearActorDTO
    {
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public IFormFile? Foto { get; set; }
    }
}
