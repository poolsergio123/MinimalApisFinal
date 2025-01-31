using AutoMapper;
using ForNewTest.IRepositorio;

namespace ForNewTest.DTO_s
{
    public class ParametrosObtenerGeneroIdDTO
    {
        public int Id { get; set; }
        public IGeneroRepositorio GeneroRepositorio { get; set; }
        public IMapper Mapper { get; set; }
    }
}
