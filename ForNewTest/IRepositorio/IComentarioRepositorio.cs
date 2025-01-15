using ForNewTest.DTO_s;
using ForNewTest.Entidades;

namespace ForNewTest.IRepositorio
{
    public interface IComentarioRepositorio
    {
        Task<List<ComentarioModel>> ObtenerComentarios(int peliculaid, PaginaDTO paginaDTO);
        Task<ComentarioModel> ObtenerPorId(int id, int peliculaid);
        Task<int> CrearComentario(ComentarioModel comentario);
        Task ActualizarComentario(ComentarioModel comentario);
        Task EliminarComentario(int id);
        Task<bool> Existe(int id);
        Task<List<ComentarioModel>> ObtenerPorCuerpo(string cuerpo,int peliculaid);
    }
}
