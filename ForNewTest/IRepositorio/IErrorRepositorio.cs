using ForNewTest.Entidades;

namespace ForNewTest.IRepositorio
{
    public interface IErrorRepositorio
    {
        Task CrearError(ErrorModel errorModel);
    }
}
