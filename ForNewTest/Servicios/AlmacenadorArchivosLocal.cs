
namespace ForNewTest.Servicios
{
    public class AlmacenadorArchivosLocal : IAlmacenadorArchivos
    {
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AlmacenadorArchivosLocal(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<string> Almacenar(string contenedor, IFormFile archivo)
        {
            var extension = System.IO.Path.GetExtension(archivo.FileName);
            var nombreArchivo = $"{Guid.NewGuid()}{extension}";
            string folder = System.IO.Path.Combine(_env.WebRootPath, contenedor);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            string ruta = System.IO.Path.Combine(folder,nombreArchivo);
            using(var ms = new MemoryStream())
            {
                await archivo.CopyToAsync(ms);
                var contenido = ms.ToArray();
                await File.WriteAllBytesAsync(ruta,contenido);
            }
            var url = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
            var urlArchivo= System.IO.Path.Combine(url,contenedor,nombreArchivo).Replace("\\","/");
            return urlArchivo;
        }

        public Task Borrar(string ruta, string contenedor)
        {
            if (string.IsNullOrEmpty(ruta))
            {
                return Task.CompletedTask;
            }

            var nombreArchivo= System.IO.Path.GetFileName(ruta);
            var directorioArchivo = System.IO.Path.Combine(_env.WebRootPath, contenedor, nombreArchivo);
            if (File.Exists(directorioArchivo))
            {
                File.Delete(directorioArchivo);
            }
            return Task.CompletedTask;
        }
    }
}
