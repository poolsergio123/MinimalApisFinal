using ForNewTest.DTO_s;
using ForNewTest.Entidades;

namespace ForNewTest.IRepositorio
{
    public interface IActorRepositorio
    {
        Task<List<ActorModel>> ObtenerActores(PaginaDTO paginaDTO);
        Task<ActorModel?> ObtenerActorPorId(int id);
        Task<int> CrearActor(ActorModel Actor);
        Task ActualizarActor(ActorModel Actor);
        Task EliminarActor(int id);
        Task<bool> Existe(int id);
        Task<List<ActorModel>> ObtenerPorNombre(string nombre);
        Task<List<int>> Existen(List<int> ids);
    }
}
