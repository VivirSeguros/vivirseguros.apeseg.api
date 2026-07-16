using System;

namespace VidaCamara.CrossCuting.Utilities
{
    public static class DateTimeExtensions
    {
        private const string DATE_FORMAT = "dd/MM/yyyy";
        private const string DATE_FORMAT_MAIL = "yyyy-MM-dd";
        public static string ToSOATDateFormat(this DateTime date)
        {
            return date.ToString(DATE_FORMAT);
        }

        public static string ToMailDateFormat(this DateTime date)
        {
            return date.ToString(DATE_FORMAT_MAIL);
        }
    }
}