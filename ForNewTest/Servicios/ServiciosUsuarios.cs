﻿using Microsoft.AspNetCore.Identity;

namespace ForNewTest.Servicios
{
    public class ServiciosUsuarios : IServiciosUsuarios
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<IdentityUser> userManager;

        public ServiciosUsuarios(IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
        }

        public async Task<IdentityUser?> ObtenerUsuario()
        {
            var emailClaim = httpContextAccessor.HttpContext!.User.Claims.Where(x => x.Type == "email").FirstOrDefault();
            if (emailClaim is null)
            {
                return null;
            }
            var email = emailClaim.Value;
            return await userManager.FindByEmailAsync(email);
        }
    }
}
