using FluentValidation;
using ForNewTest.DTO_s;

namespace ForNewTest.Validaciones
{
    public class EditarClaimDTOValidador : AbstractValidator<EditarClaimDTO>
    {
        public EditarClaimDTOValidador()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage(Utilidades.CampoRequeridoMensaje)
                               .MaximumLength(256).WithMessage(Utilidades.MaxCaracteresMensaje)
                               .EmailAddress().WithMessage(Utilidades.EmailMensaje);
        }
    }
}
