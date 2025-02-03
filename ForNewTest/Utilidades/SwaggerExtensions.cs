using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace ForNewTest.Utilidades
{
    public static class SwaggerExtensions
    {
        public static Tbuilder AgregarParametrosPeliculasFiltroAOpenApi<Tbuilder>(this Tbuilder tbuilder)where Tbuilder: IEndpointConventionBuilder
        {
            return tbuilder.WithOpenApi(opciones =>
            {
                opciones.Parameters.Add(new OpenApiParameter
                {
                    Name = "pagina",
                    In = ParameterLocation.Query,
                    Schema = new OpenApiSchema
                    {
                        Type = "integer",
                        Default= new OpenApiInteger(1)
                    }
                });

                opciones.Parameters.Add(new OpenApiParameter
                {
                    Name = "recordsPorPagina",
                    In = ParameterLocation.Query,
                    Schema = new OpenApiSchema
                    {
                        Type = "integer",
                        Default = new OpenApiInteger(10)
                    }
                });

                opciones.Parameters.Add(new OpenApiParameter
                {
                    Name = "titulo",
                    In = ParameterLocation.Query,
                    Schema = new OpenApiSchema
                    {
                        Type = "string",
                    }
                });

                opciones.Parameters.Add(new OpenApiParameter
                {
                    Name="enCines",
                    In= ParameterLocation.Query,
                    Schema=new OpenApiSchema
                    {
                        Type= "boolean",
                    }
                });

                opciones.Parameters.Add(new OpenApiParameter
                {
                    Name = "proximosEstrenos",
                    In = ParameterLocation.Query,
                    Schema = new OpenApiSchema
                    {
                        Type = "boolean",
                    }
                });

                opciones.Parameters.Add(new OpenApiParameter
                {
                    Name = "generoModelId",
                    In = ParameterLocation.Query,
                    Schema = new OpenApiSchema
                    {
                        Type = "integer",
                    }
                });
                opciones.Parameters.Add(new OpenApiParameter
                {
                    Name = "campoOrdenar",
                    In = ParameterLocation.Query,
                    Schema = new OpenApiSchema
                    {
                        Type = "string",
                        Enum= new List<IOpenApiAny> { new OpenApiString("Titulo"),
                                                      new OpenApiString("FechaLanzamiento"),
                                                      new OpenApiString("Id")}
                    }
                });
                opciones.Parameters.Add(new OpenApiParameter
                {
                    Name = "ordenarAscendente",
                    In = ParameterLocation.Query,
                    Schema = new OpenApiSchema
                    {
                        Type = "boolean",
                        Default= new OpenApiBoolean(true)
                    }
                });


                return opciones;
            });
        }

        public static Tbuilder AgregarParametrosAOpenApi<Tbuilder>(this Tbuilder tbuilder) where Tbuilder : IEndpointConventionBuilder
        {
            return tbuilder.WithOpenApi(opciones =>
            {
                opciones.Parameters.Add(new OpenApiParameter
                {
                    Name = "pagina",
                    In = ParameterLocation.Query,
                    Schema = new OpenApiSchema
                    {
                        Type = "integer",
                        Default = new OpenApiInteger(1)
                    }
                });

                opciones.Parameters.Add(new OpenApiParameter
                {
                    Name = "recordsPorPagina",
                    In = ParameterLocation.Query,
                    Schema = new OpenApiSchema
                    {
                        Type = "integer",
                        Default = new OpenApiInteger(10)
                    }
                });

                
                return opciones;
            });
        }
    }
}
