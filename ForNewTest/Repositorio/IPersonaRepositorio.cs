using ForNewTest.DTO_s;
using ForNewTest.Entidades;

namespace ForNewTest.Repositorio
{
    public interface IPersonaRepositorio
    {
        Task<List<PersonaModel>> ObtenerPersonas(PaginaDTO paginaDTO);

    }
}
