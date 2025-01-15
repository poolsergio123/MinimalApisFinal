using FluentValidation;
using ForNewTest.DTO_s;

namespace ForNewTest.Validaciones
{
    public class CrearComentarioDTOValidacion:AbstractValidator<CrearComentarioDTO>
    {
        public CrearComentarioDTOValidacion() {
            RuleFor(x => x.Cuerpo).NotEmpty().WithMessage(Utilidades.CampoRequeridoMensaje);

        }
    }
}
