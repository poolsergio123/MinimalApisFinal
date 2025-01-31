using ForNewTest.Utilidades;

namespace ForNewTest.DTO_s
{
    public class PeliculasFiltrarDTO
    {
        public int Pagina { get; set; }
        public int RecordsPorPagina { get; set; }

        public PaginaDTO PaginaDTO {
            get { return new PaginaDTO() { Pagina = Pagina, RegistrosPorPagina = RecordsPorPagina }; }
        }

        public string? Titulo { get; set; }
        public int GeneroModelId { get; set; }
        public bool EnCines { get; set; }
        public bool ProximosEstrenos { get; set; }
        public string? CampoOrdenar { get; set; }
        public bool? OrdenarAscendente { get; set; } = true;



        public static ValueTask<PeliculasFiltrarDTO> BindAsync(HttpContext httpContext)
        {
            var pagina = httpContext.ExtraerValorODefault(nameof(Pagina),1);
            var recordsPorPagina = httpContext.ExtraerValorODefault(nameof(RecordsPorPagina),10);

            var generoModelId = httpContext.ExtraerValorODefault(nameof(GeneroModelId),0);


            var titulo = httpContext.ExtraerValorODefault(nameof(Titulo),string.Empty);

            var enCines = httpContext.ExtraerValorODefault(nameof(EnCines),false);

            var proximosEstrenos = httpContext.ExtraerValorODefault(nameof(ProximosEstrenos),false);

            var campoOrdernar = httpContext.ExtraerValorODefault(nameof(CampoOrdenar), string.Empty);
            var ordenarAscendente = httpContext.ExtraerValorODefault(nameof(OrdenarAscendente),false);

            var resultado = new PeliculasFiltrarDTO
            {
                Pagina = pagina,
                RecordsPorPagina = recordsPorPagina,
                GeneroModelId = generoModelId,
                Titulo = titulo,
                EnCines = enCines,
                ProximosEstrenos = proximosEstrenos,
                CampoOrdenar = campoOrdernar,
                OrdenarAscendente = ordenarAscendente,

            };
            return ValueTask.FromResult(resultado);
        }
    }
}
