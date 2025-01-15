namespace ForNewTest.Validaciones
{
    public static class Utilidades
    {
        public static string CampoRequeridoMensaje = "El campo {PropertyName} es requerido.";
        public static string MaxCaracteresMensaje = "El campo {PropertyName} solo acepta {MaxLength} caracteres.";
        public static string PrimeraLetraMensaje = "El campo {PropertyName} debe iniciar con letra mayuscula.";
        public static string GreatherThanOrEqualToMensaje(DateTime dateTime)
        {
            return "El campo {PropertyName} solo acepta fechas mayores a " + dateTime.ToString("yyyy-MM-dd");
        }
        public static bool PrimeraLetraEnMayuscula(string valor)
        {
            if (string.IsNullOrEmpty(valor))
            {
                return true;
            }

            var primeraLetra = valor[0].ToString();

            return primeraLetra == primeraLetra.ToUpper();
        }
    }
}
