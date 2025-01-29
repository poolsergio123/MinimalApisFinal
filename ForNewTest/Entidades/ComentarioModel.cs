using Microsoft.AspNetCore.Identity;

namespace ForNewTest.Entidades
{
    public class ComentarioModel
    {
        public int Id { get; set; }
        public string Cuerpo { get; set; }
        public int PeliculaModelId { get; set; }
        public string UsuarioModelId { get; set; } = null!;
        public IdentityUser Usuarios { get; set; } = null!;
    }

    public class Error
    {
        public string error { get; set; }
    }
}
