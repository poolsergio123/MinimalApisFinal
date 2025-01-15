using Microsoft.EntityFrameworkCore;

namespace ForNewTest.Utilidades
{
    public static class HttpContextExtensions
    {
        public static async Task ParametrosEnCabecera<T>(this HttpContext httpContext, IQueryable<T> queryable)
        {
            if (httpContext is null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }
            double cantidad = await queryable.CountAsync();

            httpContext.Response.Headers.Append("CantidadRegistrosTotales",cantidad.ToString());
        }
    }
}
