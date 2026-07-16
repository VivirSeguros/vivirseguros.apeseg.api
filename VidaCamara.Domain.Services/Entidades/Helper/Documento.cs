using System;
using System.Collections.Generic;
using System.Text;

namespace VidaCamara.Domain.Services.Entidades.Helper
{
    public class Documento
    {


    }

    public class DocumentoParam
    {
        public int IdAuto { get; set; }
        public string Descripcion { get; set; }
        public string UrlDocumento { get; set; }
        public int Paso { get; set; }
    }
}
