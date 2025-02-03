using ForNewTest.Contexto;
using ForNewTest.Entidades;

namespace ForNewTest.GraphQL
{
    public class Query
    {
        public IQueryable<GeneroModel> ObtenerGenero([Service] AplicationDBContext aplicationDBContext) => aplicationDBContext.Generos;
        
    }
}
