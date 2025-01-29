using Microsoft.AspNetCore.Identity;

namespace ForNewTest.Servicios
{
    public interface IServiciosUsuarios
    {
        Task<IdentityUser?> ObtenerUsuario();
    }
}