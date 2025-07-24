using ForNewTest.Contexto;
using ForNewTest.DTO_s;
using ForNewTest.Entidades;
using ForNewTest.IRepositorio;
using ForNewTest.Utilidades;
using Microsoft.EntityFrameworkCore;

namespace ForNewTest.Repositorio
{
    public class PersonaRepositorio : IPersonaRepositorio
    {
        private readonly AplicationDBContext _context;
        private readonly HttpContext httpContext;

        public PersonaRepositorio(AplicationDBContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            httpContext = contextAccessor.HttpContext!;
        }

        public async Task<List<PersonaModel>> ObtenerPersonas(PaginaDTO paginaDTO)
        {

            // Ejecuta el SP y trae los resultados a memoria
            var personas = await _context.Personas
                                         .FromSqlRaw("EXEC spListarPersona")
                                         .AsNoTracking()
                                         .ToListAsync();

            // Lógica de paginación en memoria
            var queryable = personas.AsQueryable();

            await httpContext.ParametrosEnCabecera(queryable); // Por si necesitas conteo total u otros headers

            return queryable
                .OrderBy(x => x.Nombre)
                .Paginar(paginaDTO)
                .ToList(); // NO necesitas ToListAsync, ya estás en memoria
            //var queryable = _context.Personas.FromSqlRaw("spListarPersona").AsEnumerable();
            //await httpContext.ParametrosEnCabecera(queryable.AsQueryable());
            //return queryable.AsQueryable().OrderBy(x => x.Nombre).Paginar(paginaDTO).ToList();
        }
    }
}
