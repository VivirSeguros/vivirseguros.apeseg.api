using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivirSeguros.CRM.Application.Commons
{
    public static class UtilHelper
    {
        public static string obtainConfig(string value)
        {
            ExeConfigurationFileMap customConfigFileMap = new ExeConfigurationFileMap();
            customConfigFileMap.ExeConfigFilename = AppDomain.CurrentDomain.BaseDirectory + "customAppSettings.xml";
            System.Configuration.Configuration customConfig = ConfigurationManager.OpenMappedExeConfiguration(customConfigFileMap, ConfigurationUserLevel.None);
            AppSettingsSection appSettings = (customConfig.GetSection("appSettings") as AppSettingsSection);
            return appSettings.Settings[value].Value;
        }

        public static void EscribirFile(string path, string pathFileName, string content)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                if (!File.Exists(pathFileName))
                {
                    File.Create(pathFileName).Dispose();

                    using (TextWriter tw = new StreamWriter(pathFileName, true))
                    {
                        tw.WriteLine(content);
                        tw.Flush();
                        tw.Close();
                        tw.Dispose();
                    }

                }
                else if (File.Exists(pathFileName))
                {
                    using (FileStream fs = new FileStream(pathFileName, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        StreamWriter write = new StreamWriter(fs);
                        write.BaseStream.Seek(0, SeekOrigin.End);
                        write.WriteLine(content);
                        write.Flush();
                        write.Close();
                        fs.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.save(null, "ERROR EN: EscribirFile()" + ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// Create a ZIP file of the files provided.
        /// </summary>
        /// <param name="fileName">The full path and name to store the ZIP file at.</param>
        /// <param name="files">The list of files to be added.</param>
        public static void CreateZipFile(string fileName, IEnumerable<string> files)
        {
            // Create and open a new ZIP file
            var zip = ZipFile.Open(fileName, ZipArchiveMode.Create);
            foreach (var file in files)
            {
                // Add the entry for each file
                zip.CreateEntryFromFile(file, Path.GetFileName(file), CompressionLevel.Optimal);
            }
            // Dispose of the object when we are done
            zip.Dispose();
        }

        public static void Copy(string sourceDir, string targetDir)
        {
            if (!Directory.Exists(targetDir))
                Directory.CreateDirectory(targetDir);

            foreach (var file in Directory.GetFiles(sourceDir))
                File.Copy(file, Path.Combine(targetDir, Path.GetFileName(file)));

            foreach (var directory in Directory.GetDirectories(sourceDir))
                Copy(directory, Path.Combine(targetDir, Path.GetFileName(directory)));
        }
    }
}
