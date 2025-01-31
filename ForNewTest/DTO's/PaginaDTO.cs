using ForNewTest.Utilidades;
using Microsoft.IdentityModel.Tokens;

namespace ForNewTest.DTO_s
{
    public class PaginaDTO
    {
        private const int paginaValorInicial = 1;
        private const int recordsPorPaginaValorInicial = 10;
        public int Pagina { get; set; } = paginaValorInicial;
        private int registrosPorPagina { get; set; } = recordsPorPaginaValorInicial;
        private readonly int MaxRegistrosPorPagina  = 50;

        public int RegistrosPorPagina
        {
            get { return registrosPorPagina; }
            set { registrosPorPagina = (value > registrosPorPagina) ? MaxRegistrosPorPagina : value; }
        }

        public static ValueTask<PaginaDTO> BindAsync(HttpContext httpContext)
        {
            var pagina = httpContext.ExtraerValorODefault(nameof(Pagina),paginaValorInicial);
            var RecordPorPagina = httpContext.ExtraerValorODefault(nameof(RegistrosPorPagina),recordsPorPaginaValorInicial);

            //var paginaInt = pagina.IsNullOrEmpty() ? paginaValorInicial : int.Parse(pagina.ToString()); 
            //var recordspaginaInt = RecordPorPagina.IsNullOrEmpty() ? recordsPorPaginaValorInicial: int.Parse(RecordPorPagina.ToString());

            var resultado = new PaginaDTO
            {
                Pagina = pagina,
                RegistrosPorPagina = RecordPorPagina,
            };

            return ValueTask.FromResult(resultado);
        }
    }
}
