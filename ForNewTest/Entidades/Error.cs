namespace ForNewTest.Entidades
{
    public class ErrorModel
    {
        public Guid Id { get; set; }
        public string MensajeError { get; set; } = null!;
        public string StackTrace { get; set; } = null!;
        public DateTime Fecha { get; set; }
    }
}
