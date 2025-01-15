using ForNewTest.Contexto;
using ForNewTest.Entidades;
using ForNewTest.IRepositorio;
using Microsoft.EntityFrameworkCore;

namespace ForNewTest.Repositorio
{
    public class GeneroRepositorio : IGeneroRepositorio
    {

        private readonly AplicationDBContext _context;
        public GeneroRepositorio(AplicationDBContext context)
        {
            _context = context;
        }
        public async Task ActualizarGenero(GeneroModel genero)
        {
            _context.Generos.Update(genero);
            await _context.SaveChangesAsync();
            return;
        }

        public async Task<int> CrearGenero(GeneroModel genero)
        {
            _context.Generos.Add(genero);
            await _context.SaveChangesAsync();
            return genero.Id;
        }

        public async Task EliminarGenero(int id)
        {
            await _context.Generos.Where(x=>x.Id == id).ExecuteDeleteAsync();
        }

        public async Task<bool> Existe(int id)
        {
            return await _context.Generos.AnyAsync(x=>x.Id == id);
        }

        public async Task<bool> Existe(int id,string valor)
        {
            return await _context.Generos.AnyAsync(x => x.Id != id&& x.Nombre == valor);
        }

        public async Task<GeneroModel> ObtenerGeneroPorId(int id)
        {
            return await _context.Generos.Where(x=>x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<GeneroModel>> ObtenerGeneros()
        {
            return await _context.Generos.ToListAsync();
        }

        public async Task<List<int>> Existen(List<int> ids)
        {
            return await _context.Generos.Where(x=>ids.Contains(x.Id)).Select(x=>x.Id).ToListAsync();
        }
    }
}
