using FluentValidation;
using ForNewTest.DTO_s;

namespace ForNewTest.Validaciones
{
    public class CrearActorDTOValidador: AbstractValidator<CrearActorDTO>
    {

        public CrearActorDTOValidador()
        {
            RuleFor(x => x.Nombre).NotEmpty().WithMessage(Utilidades.CampoRequeridoMensaje)
                                    .MaximumLength(75).WithMessage(Utilidades.MaxCaracteresMensaje);

            var fechaMinima = new DateTime(1900,1,1);

            RuleFor(x=>x.FechaNacimiento).GreaterThanOrEqualTo(fechaMinima).WithMessage(Utilidades.GreatherThanOrEqualToMensaje(fechaMinima));
        }
    }
}
