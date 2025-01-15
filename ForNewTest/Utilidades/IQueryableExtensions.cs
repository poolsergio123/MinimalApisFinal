using ForNewTest.DTO_s;

namespace ForNewTest.Utilidades
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> Paginar<T>(this IQueryable<T> queryable,PaginaDTO paginaDTO)
        {
            return queryable.Skip((paginaDTO.Pagina-1)* paginaDTO.RegistrosPorPagina).Take(paginaDTO.RegistrosPorPagina);
        }
    }
}
