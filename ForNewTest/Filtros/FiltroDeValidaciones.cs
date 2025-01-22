
using FluentValidation;
using ForNewTest.DTO_s;

namespace ForNewTest.Filtros
{
    public class FiltroDeValidaciones<T> : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var validador = context.HttpContext.RequestServices.GetService<IValidator<T>>();
            if (validador is null)
            {
                return await next(context);
            }

            var insumo = context.Arguments.OfType<T>().FirstOrDefault();
            if (insumo is null)
            {
                return TypedResults.Problem("No se reconoce la Entidad.");
            }

            var validador2 = await validador.ValidateAsync(insumo);
            if (!validador2.IsValid)
            {
                return TypedResults.ValidationProblem(validador2.ToDictionary());
            }

            return await next(context);
        }

    }
}
