﻿using FluentValidation;
using ForNewTest.DTO_s;
using ForNewTest.IRepositorio;

namespace ForNewTest.Validaciones
{
    public class CrearGeneroDTOValidador:AbstractValidator<CrearGeneroDTO>
    {
        public CrearGeneroDTOValidador(IGeneroRepositorio generoRepositorio,IHttpContextAccessor httpContextAccessor) {

            var ruta = httpContextAccessor.HttpContext?.Request.RouteValues["id"];
            var id = 0;
            if (ruta is string param)
            {
                int.TryParse(param,out id);
            }
            RuleFor(x => x.Nombre).NotEmpty().WithMessage(Utilidades.CampoRequeridoMensaje)
                                  .MaximumLength(100).WithMessage(Utilidades.MaxCaracteresMensaje)
                                  .Must(Utilidades.PrimeraLetraEnMayuscula).WithMessage(Utilidades.PrimeraLetraMensaje)
                                  .MustAsync(async (nombre,_) =>
                                  {
                                      var existe = await generoRepositorio.Existe(id, nombre);
                                      return !existe;
                                  }).WithMessage(g=>$"Ya existe un genero con el nombre {g.Nombre}.");
        }

        
    }
}