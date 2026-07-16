using System.Collections.Generic;

namespace VidaCamara.Domain.Services.Entidades.Helper
{
    public class CrearDocumento
    {
        public Dictionary<string, string> parameters { get; set; }
        public string[] propertyNamesProcces { get; set; }
        public string pathToSaveFolderAdd { get; set; }
        public string fileName { get; set; }
        public string pathFileTemplate { get; set; }

    }
}