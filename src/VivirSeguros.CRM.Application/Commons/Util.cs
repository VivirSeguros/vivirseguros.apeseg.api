using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivirSeguros.CRM.Application.Commons
{
    public static class Util
    {
        public static void WriteToJsonFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
        {
            TextWriter writer = null;
            try
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);

                var contentsToWriteToFile = JsonConvert.SerializeObject(objectToWrite, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                writer = new StreamWriter(filePath, append);
                writer.Write(contentsToWriteToFile);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        public static string WriteToJson<T>(T objectToWrite) where T : new()
        {
            return JsonConvert.SerializeObject(objectToWrite, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        public static string ConvertToBase64(string pathNameFile)
        {
            String file = "";
            try
            {
                Byte[] bytes = File.ReadAllBytes(pathNameFile);
                file = Convert.ToBase64String(bytes);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return file;
        }

        public static void DeleteFile(string pathNameFile)
        {
            if (File.Exists(pathNameFile))
                File.Delete(pathNameFile);
        }
    }
}
