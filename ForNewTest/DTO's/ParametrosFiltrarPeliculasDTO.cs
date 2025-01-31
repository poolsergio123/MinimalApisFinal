using AutoMapper;
using ForNewTest.IRepositorio;

namespace ForNewTest.DTO_s
{
    public class ParametrosFiltrarPeliculasDTO
    {
        public IPeliculaRepositorio? Repositorio { get; set; }
        public IMapper? Mapper { get; set; }
    }
}
