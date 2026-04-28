using System;
using System.Globalization;

namespace VidaCamara.CrossCuting.Utilities
{
    public static class DateHelper
    {
        public static DateTime ConvertStringToDateTime(string dateString, string format = "yyyy-MM-dd")
        {
            if (string.IsNullOrWhiteSpace(dateString))
                throw new ArgumentException("La cadena de fecha no puede estar vacía.");

            try
            {
                return DateTime.ParseExact(dateString, format, CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                throw new FormatException($"La cadena '{dateString}' no tiene el formato esperado '{format}'.");
            }
        }

        public static string ObtenerNombreDelMes(DateTime fecha)
        {
            return fecha.ToString("MMMM", new CultureInfo("es-ES"));
        }

        public static string ObtenerFechaFormatoLargo(DateTime fecha)
        {
            return fecha.ToString("d 'de' MMMM 'de' yyyy", new CultureInfo("es-ES"));
        }
    }
}