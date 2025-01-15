using ForNewTest.Contexto;
using ForNewTest.DTO_s;
using ForNewTest.Entidades;
using ForNewTest.IRepositorio;
using ForNewTest.Utilidades;
using Microsoft.EntityFrameworkCore;

namespace ForNewTest.Repositorio
{
    public class ActorRepositorio : IActorRepositorio
    {

        private readonly AplicationDBContext _context;
        private readonly HttpContext httpContext;

        public ActorRepositorio(AplicationDBContext context,IHttpContextAccessor contextAccessor)
        {
            _context = context;
            httpContext = contextAccessor.HttpContext!;
        }
        public async Task ActualizarActor(ActorModel Actor)
        {
            _context.Actores.Update(Actor);
            await _context.SaveChangesAsync();
        }

        public async Task<int> CrearActor(ActorModel Actor)
        {
            _context.Actores.Add(Actor);
            await _context.SaveChangesAsync();
            return Actor.Id;
        }

        public async Task EliminarActor(int id)
        {
            await _context.Actores.Where(x=>x.Id == id).ExecuteDeleteAsync();
        }

        public async Task<bool> Existe(int id)
        {
            return await _context.Actores.AnyAsync(x=>x.Id == id);
        }

        public async Task<List<int>> Existen(List<int> ids)
        {
            return await _context.Actores.Where(x => ids.Contains(x.Id)).Select(x => x.Id).ToListAsync();
        }

        public async Task<List<ActorModel>> ObtenerPorNombre(string nombre)
        {
            return await _context.Actores.Where(x => x.Nombre.Contains(nombre)).OrderBy(x => x.Nombre).ToListAsync();
        }

        public async Task<List<ActorModel>> ObtenerActores(PaginaDTO paginaDTO)
        {
            var queryable = _context.Actores.AsQueryable();
            await httpContext.ParametrosEnCabecera(queryable);
            return await queryable.OrderBy(x=> x.Nombre).Paginar(paginaDTO).ToListAsync();
        }

        public async Task<ActorModel> ObtenerActorPorId(int id)
        {
            
            return await _context.Actores.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

    }
}
