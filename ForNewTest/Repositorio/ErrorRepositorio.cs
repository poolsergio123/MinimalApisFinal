using ForNewTest.Contexto;
using ForNewTest.Entidades;
using ForNewTest.IRepositorio;
using Microsoft.EntityFrameworkCore;

namespace ForNewTest.Repositorio
{
    public class ErrorRepositorio : IErrorRepositorio
    {
        private readonly AplicationDBContext _context;

        public ErrorRepositorio(AplicationDBContext context) {
            _context = context;
        }


        public async Task CrearError(ErrorModel errorModel)
        {
            _context.Add(errorModel);
            await _context.SaveChangesAsync();
        }
    }
}
