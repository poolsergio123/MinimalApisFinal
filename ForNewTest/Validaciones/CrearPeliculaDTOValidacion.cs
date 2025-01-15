using FluentValidation;
using ForNewTest.DTO_s;

namespace ForNewTest.Validaciones
{
    public class CrearPeliculaDTOValidacion : AbstractValidator<CrearPeliculaDTO>
    {

        public CrearPeliculaDTOValidacion() {
            RuleFor(x => x.Titulo).NotEmpty().WithMessage(Utilidades.CampoRequeridoMensaje)
                                    .MaximumLength(200).WithMessage(Utilidades.MaxCaracteresMensaje);
        }
    }
}
