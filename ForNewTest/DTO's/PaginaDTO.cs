namespace ForNewTest.DTO_s
{
    public class PaginaDTO
    {
        public int Pagina { get; set; } = 1;
        private int registrosPorPagina { get; set; } = 10;
        private readonly int MaxRegistrosPorPagina  = 50;

        public int RegistrosPorPagina
        {
            get { return registrosPorPagina; }
            set { registrosPorPagina = (value > registrosPorPagina) ? MaxRegistrosPorPagina : value; }
        }
    }
}
