
using AutoMapper;
using ForNewTest.IRepositorio;

namespace ForNewTest.Filtros
{
    public class FiltrosDePrueba : IEndpointFilter
    {
        public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var resultado = await next(context);

            var param1 = context.Arguments.OfType<int>().FirstOrDefault();
            var param2 = (IGeneroRepositorio)context.Arguments[1];
            var param3 = (IMapper)context.Arguments[2];
            //Codigo despues del endpoint
            return resultado;
        }
    }
}
