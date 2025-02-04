using ForNewTest.Contexto;
using ForNewTest.Entidades;

namespace ForNewTest.GraphQL
{
    public class Query
    {
        [Serial]
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<GeneroModel> ObtenerGeneros([Service] AplicationDBContext aplicationDBContext) => aplicationDBContext.Generos;


        [Serial]
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<PeliculaModel> ObtenerPeliculas([Service] AplicationDBContext aplicationDBContext) => aplicationDBContext.Peliculas;


        [Serial]
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<ActorModel> ObtenerActores([Service] AplicationDBContext aplicationDBContext) => aplicationDBContext.Actores;


    }
}
