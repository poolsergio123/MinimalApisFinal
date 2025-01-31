﻿namespace ForNewTest.Utilidades
{
    public static class HttpContextExtensionsUtilidades
    {
        public static T ExtraerValorODefault<T>(this HttpContext httpContext, string nombreDelCampo,T valorPorDefecto) where T : IParsable<T>
        {
            var valor = httpContext.Request.Query[nombreDelCampo];
            if (!valor.Any())
            {
                return valorPorDefecto;
            }
            return T.Parse(valor!,null);
        }
    }
}
