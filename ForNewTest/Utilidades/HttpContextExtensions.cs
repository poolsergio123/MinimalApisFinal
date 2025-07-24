using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

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
            // Si el proveedor es asincrónico (EF), usa CountAsync

            double cantidad;
            if (queryable.Provider is IAsyncQueryProvider)
            {
                cantidad = await queryable.CountAsync();
            }
            else
            {
                cantidad = queryable.Count(); // En memoria
            }

            httpContext.Response.Headers.Append("CantidadRegistrosTotales",cantidad.ToString());
        }
    }
}
