using AutoMapper;
using ForNewTest.Contexto;
using ForNewTest.DTO_s;
using ForNewTest.Entidades;
using ForNewTest.IRepositorio;
using ForNewTest.Utilidades;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ForNewTest.Repositorio
{
    public class PeliculaRepositorio :IPeliculaRepositorio
    {
        private readonly AplicationDBContext _context;
        private readonly HttpContext _httpContext;
        private readonly IMapper _mapper;

        public PeliculaRepositorio(AplicationDBContext dBContext, IHttpContextAccessor contextAccessor,IMapper mapper)
        {
            _context = dBContext;
            _httpContext = contextAccessor.HttpContext!;
            _mapper = mapper;
        }

        public async Task<List<PeliculaModel>> ObtenerPeliculas(PaginaDTO paginaDTO)
        {

            var queryable = _context.Peliculas.AsQueryable();
            await _httpContext.ParametrosEnCabecera(queryable);
            return await queryable.OrderBy(x => x.Titulo).Paginar(paginaDTO).ToListAsync();

        }

        public async Task<int> CrearPelicula(PeliculaModel peliculaModel)
        {
            _context.Peliculas.Add(peliculaModel);
            await _context.SaveChangesAsync();
            return peliculaModel.Id;
        }

        public async Task<PeliculaModel> ObtenerPorId(int id)
        {
            return await _context.Peliculas.Include(x=> x.Comentarios)
                .Include(x=>x.GeneroPeliculas)
                    .ThenInclude(x=>x.Genero)
                .Include(x=>x.ActoresPeliculas.OrderBy(x=>x.Orden))
                    .ThenInclude(x => x.Actor)
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Actualizar(PeliculaModel peliculaModel)
        {
            _context.Peliculas.Update(peliculaModel);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Existe(int id)
        {
            return await _context.Peliculas.AnyAsync(x => x.Id == id);
        }

        public async Task EliminarPelicular(int id)
        {
            await _context.Peliculas.Where(x => x.Id == id).ExecuteDeleteAsync();
        }

        public async Task<List<PeliculaModel>> ObtenerPorNombre(string titulo)
        {
            return await _context.Peliculas.Where(x => x.Titulo.Contains(titulo)).OrderBy(x => x.Titulo).ToListAsync();
        }

        public async Task AsignarGeneros(int id, List<int> generosId)
        {
            var pelicula = await _context.Peliculas.Include(x => x.GeneroPeliculas).FirstOrDefaultAsync(x => x.Id == id);
            if (pelicula is null)
            {
                throw new ArgumentException($"No existe una pelicula con el id {id}");
            }

            var generosPeliculas = generosId.Select(generoId => new GeneroPelicula() {GeneroModelId = generoId });
            pelicula.GeneroPeliculas = _mapper.Map(generosPeliculas,pelicula.GeneroPeliculas);

            await _context.SaveChangesAsync();

        }

        public async Task AsignarActores(int id, List<ActorPelicula> actores)
        {
            for (int i = 1; i <= actores.Count; i++)
            {
                actores[i-1].Orden = i;
            }
            var pelicula = await _context.Peliculas.Include(x => x.ActoresPeliculas).FirstOrDefaultAsync(x=>x.Id==id);
            if (pelicula is null)
            {
                throw new ArgumentException($"No existe la pelicula con id {id}");
            }

            pelicula.ActoresPeliculas = _mapper.Map(actores,pelicula.ActoresPeliculas);
            await _context.SaveChangesAsync();
        }

        public async Task<List<PeliculaModel>> Filtrar(PeliculasFiltrarDTO peliculasFiltrarDTO)
        {
            var peliculasQueryable = _context.Peliculas.AsQueryable();
            if (!string.IsNullOrEmpty(peliculasFiltrarDTO.Titulo))
            {
                peliculasQueryable = peliculasQueryable.Where(p => p.Titulo.Contains(peliculasFiltrarDTO.Titulo));
            }

            if (peliculasFiltrarDTO.EnCines)
            {
                peliculasQueryable = peliculasQueryable.Where(p => p.EnCines);
            }

            if (peliculasFiltrarDTO.ProximosEstrenos)
            {
                var fecha = DateTime.Now;
                peliculasQueryable = peliculasQueryable.Where(x => x.FechaLanzamiento > fecha);
            }

            if (peliculasFiltrarDTO.GeneroModelId != 0)
            {
                peliculasQueryable = peliculasQueryable.Where(p => p.GeneroPeliculas.Select(gp => gp.GeneroModelId).Contains(peliculasFiltrarDTO.GeneroModelId));
            }

            await _httpContext.ParametrosEnCabecera(peliculasQueryable);
            var peliculas = await peliculasQueryable.Paginar(peliculasFiltrarDTO.PaginaDTO).ToListAsync();

            return peliculas;
        }
    }
}
