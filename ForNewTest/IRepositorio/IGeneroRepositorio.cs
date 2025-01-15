using ForNewTest.Entidades;

namespace ForNewTest.IRepositorio
{
    public interface IGeneroRepositorio
    {
        Task<List<GeneroModel>> ObtenerGeneros();
        Task<GeneroModel?> ObtenerGeneroPorId(int id);
        Task<int> CrearGenero(GeneroModel genero);
        Task ActualizarGenero(GeneroModel genero);
        Task EliminarGenero(int id);
        Task<bool> Existe(int id);
        Task<List<int>> Existen(List<int> ids);
        Task<bool> Existe(int id, string valor);
    }
}
