
using FluentValidation;
using ForNewTest.DTO_s;

namespace ForNewTest.Filtros
{
    public class FiltroDeGeneros : IEndpointFilter
    {
        public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var validador = context.HttpContext.RequestServices.GetService<IValidator<CrearGeneroDTO>>();
            if (validador is null)
            {
                return await next(context);
            }

            var insumoAValidar = context.Arguments.OfType<CrearGeneroDTO>().FirstOrDefault();

            if (insumoAValidar is null)
            {
                return TypedResults.Problem("No pudo ser validada la entidad a validar.");
            }

            var resultadoVal = await validador.ValidateAsync(insumoAValidar);

            if (!resultadoVal.IsValid) {
                return TypedResults.ValidationProblem(resultadoVal.ToDictionary());
            }
            return await next(context);

        }
    }
}
