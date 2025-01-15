using ForNewTest.Contexto;
using ForNewTest.DTO_s;
using ForNewTest.Entidades;
using ForNewTest.IRepositorio;
using ForNewTest.Utilidades;
using Microsoft.EntityFrameworkCore;

namespace ForNewTest.Repositorio
{
    public class ComentarioRepositorio : IComentarioRepositorio
    {
        private readonly AplicationDBContext _context;
        private readonly HttpContext _httpcontext;

        public ComentarioRepositorio(AplicationDBContext context,IHttpContextAccessor httpContextAccessor) { 
            _context = context;
            _httpcontext = httpContextAccessor.HttpContext!;
        }
        public async Task ActualizarComentario(ComentarioModel comentario)
        {
            _context.Update(comentario);
            await _context.SaveChangesAsync();
        }

        public async Task<int> CrearComentario(ComentarioModel comentario)
        {
            _context.Comentarios.Add(comentario);
            await _context.SaveChangesAsync();
            return comentario.Id;
        }

        public async Task EliminarComentario(int id)
        {
            await _context.Comentarios.Where(x => x.Id == id).ExecuteDeleteAsync();
        }

        public async Task<bool> Existe(int id)
        {
            return await _context.Comentarios.AnyAsync(x => x.Id == id);
        }

        public async Task<List<ComentarioModel>> ObtenerComentarios(int peliculaid,PaginaDTO paginaDTO)
        {
            var queryable = _context.Comentarios.AsQueryable();
            await _httpcontext.ParametrosEnCabecera(queryable);

            return await queryable.Where(x=>x.PeliculaModelId == peliculaid).OrderBy(x=> x.Id).Paginar(paginaDTO).ToListAsync();
        }

        public async Task<List<ComentarioModel>> ObtenerPorCuerpo(string cuerpo, int peliculaid)
        {
            return await _context.Comentarios.Where(x => x.Cuerpo.Contains(cuerpo) && x.PeliculaModelId == peliculaid).OrderBy(x => x.Cuerpo).ToListAsync();
        }

        public async Task<ComentarioModel> ObtenerPorId(int id,int peliculaid)
        {
            return await _context.Comentarios.AsNoTracking().FirstOrDefaultAsync(x=>x.Id ==id && x.PeliculaModelId == peliculaid);
        }
    }
}
