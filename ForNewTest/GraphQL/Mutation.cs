using AutoMapper;
using ForNewTest.DTO_s;
using ForNewTest.Entidades;
using ForNewTest.IRepositorio;
using HotChocolate.Authorization;

namespace ForNewTest.GraphQL
{
    public class Mutation
    {
        [Serial]
        [Authorize(Policy ="esadmin")]
        public async Task<GeneroDTO> InsertarGenero([Service]IGeneroRepositorio generoRepositorio, [Service]IMapper mapper, CrearGeneroDTO crearGeneroDTO)
        {
            var genero = mapper.Map<GeneroModel>(crearGeneroDTO);

            int id=await generoRepositorio.CrearGenero(genero);

            var generoDTO = mapper.Map<GeneroDTO>(genero);
            return generoDTO;
        }

        [Serial]
        [Authorize(Policy = "esadmin")]

        public async Task<GeneroDTO?> ActualizarGenero([Service] IGeneroRepositorio generoRepositorio, [Service] IMapper mapper, ActualizarGeneroDTO actualizarGenero)
        {
            var existe = await generoRepositorio.Existe(actualizarGenero.Id);
            if (!existe)
            {
                return null;
            }
            var genero = mapper.Map<GeneroModel>(actualizarGenero);
            await generoRepositorio.ActualizarGenero(genero);
            var generoDTO = mapper.Map<GeneroDTO>(genero);

            return generoDTO;
        }

        [Serial]
        [Authorize(Policy = "esadmin")]
        public async Task<bool?> EliminarGenero([Service] IGeneroRepositorio generoRepositorio,int id)
        {
            var existe = await generoRepositorio.Existe(id);
            if (!existe)
            {
                return null;
            }
            await generoRepositorio.EliminarGenero(id);

            return true;
        }
    }
}
