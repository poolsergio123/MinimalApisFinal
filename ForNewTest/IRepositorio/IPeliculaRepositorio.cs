using ForNewTest.DTO_s;
using ForNewTest.Entidades;

namespace ForNewTest.IRepositorio
{
    public interface IPeliculaRepositorio
    {
        Task<List<PeliculaModel>> ObtenerPeliculas(PaginaDTO paginaDTO);
        Task<int> CrearPelicula(PeliculaModel peliculaModel);
        Task<PeliculaModel> ObtenerPorId(int id);
        Task Actualizar(PeliculaModel peliculaModel);
        Task<bool> Existe(int id);
        Task EliminarPelicular(int id);
        Task<List<PeliculaModel>> ObtenerPorNombre(string nombre);
        Task AsignarGeneros(int id, List<int> generosId);
        Task AsignarActores(int id, List<ActorPelicula> actores);
    }
}
