﻿using FluentValidation;
using ForNewTest.DTO_s;

namespace ForNewTest.Validaciones
{
    public class CredencialesUsuarioDTOValidador: AbstractValidator<CredencialesUsuarioDTO>
    {
        public CredencialesUsuarioDTOValidador()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage(Utilidades.CampoRequeridoMensaje)
                                 .MaximumLength(256).WithMessage(Utilidades.MaxCaracteresMensaje)
                                 .EmailAddress().WithMessage(Utilidades.EmailMensaje);

            RuleFor(x => x.Password).NotEmpty().WithMessage(Utilidades.CampoRequeridoMensaje);
        }
    }
}
