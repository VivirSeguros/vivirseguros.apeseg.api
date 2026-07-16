using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace VidaCamara.Masivos.Services.Extensions
{
    public static class ControllerExtensions
    {
        #region Public members

        public static T ConvertDataToDto<T>(this Controller controller, string value)
        {
            try
            {
                var strValue = DecodeBase64ToString(Convert.ToBase64String(Encoding.Convert(Encoding.GetEncoding("ISO-8859-1"), Encoding.UTF8, Convert.FromBase64String(value))));
                return JsonConvert.DeserializeObject<T>(strValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static T ConvertDataToDtoCarater<T>(this Controller controller, string value)
        {
            var strValue = DecodeBase64ToString(Convert.ToBase64String(Encoding.Convert(Encoding.GetEncoding("ISO-8859-1"), Encoding.UTF8, Convert.FromBase64String(value))));
            return JsonConvert.DeserializeObject<T>(strValue);
        }

        public static T ConvertDataToDtoCar<T>(this Controller controller, string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        #endregion

        #region Private members

        private static string DecodeBase64ToString(string value)
        {
            var bytesValue = Convert.FromBase64String(value);

            return Encoding.UTF8.GetString(bytesValue);
        }

        #endregion
    }
}
