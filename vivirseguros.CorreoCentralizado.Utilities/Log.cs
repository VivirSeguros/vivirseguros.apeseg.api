using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vivirseguros.CorreoCentralizado.Utilities
{
    public class Log
    {
        public static void save(object obj, string value)
        {
            StreamWriter sw = null;
            try
            {
                string fecha = System.DateTime.Now.ToString("yyyyMMdd");
                string hora = System.DateTime.Now.ToString("HH:mm:ss");
                //ring path =  HttpContext.Current.Request.MapPath("~/log/" + fecha + ".txt");
                string path = UtilHelper.obtainConfig("rutaLog");

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                string pathName = path + "//Trace_" + fecha + ".txt";
                sw = new StreamWriter(pathName, true);

                StackTrace stacktrace = new StackTrace();
                if (obj is null)
                    sw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy") + " " + hora + " | " + " | Método: " + stacktrace.GetFrame(2).GetMethod().Name + " | " + value);
                else
                    sw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy") + " " + hora + " | " + obj.GetType().FullName + " | Método: " + stacktrace.GetFrame(2).GetMethod().Name + " | " + value);
                /*sw.WriteLine(obj.GetType().FullName + " " + hora);z
                sw.WriteLine(stacktrace.GetFrame(1).GetMethod().Name + " - " + msj);
                sw.WriteLine("");*/

                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            {
                if (sw != null) sw.Close();
            }
        }

        public static void saveFirstLine()
        {
            StreamWriter sw = null;
            try
            {
                string fecha = System.DateTime.Now.ToString("yyyyMMdd");
                string hora = System.DateTime.Now.ToString("HH:mm:ss");
                //ring path =  HttpContext.Current.Request.MapPath("~/log/" + fecha + ".txt");
                string path = UtilHelper.obtainConfig("rutaLog");

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                string pathName = path + "//Trace_" + fecha + ".txt";
                sw = new StreamWriter(pathName, true);

                StackTrace stacktrace = new StackTrace();
                sw.WriteLine("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                /*sw.WriteLine(obj.GetType().FullName + " " + hora);z
                sw.WriteLine(stacktrace.GetFrame(1).GetMethod().Name + " - " + msj);
                sw.WriteLine("");*/

                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            {
                if (sw != null) sw.Close();
            }
        }

        public static void error(object obj, string value)
        {
            StreamWriter sw = null;
            try
            {
                string fecha = System.DateTime.Now.ToString("yyyyMMdd");
                string hora = System.DateTime.Now.ToString("HH:mm:ss");
                //ring path =   HttpContext.Current.Request.MapPath("~/log/" + fecha + ".txt");
                string path = UtilHelper.obtainConfig("rutaLog");

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                string pathName = path + "//Error_" + fecha + ".txt";
                sw = new StreamWriter(pathName, true);

                StackTrace stacktrace = new StackTrace();
                sw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy") + " " + hora + " | " + obj.GetType().FullName + " | Método: " + stacktrace.GetFrame(1).GetMethod().Name + " | " + value);
                /*sw.WriteLine(obj.GetType().FullName + " " + hora);
                sw.WriteLine(stacktrace.GetFrame(1).GetMethod().Name + " - " + msj);
                sw.WriteLine("");*/

                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            {
                if (sw != null) sw.Close();
            }
        }
    }
}
