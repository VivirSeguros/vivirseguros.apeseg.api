using System;
using System.Collections.Generic;
using System.Text;

namespace VidaCamara.Domain.Services.Entidades.Helper
{
    public class Correo
    {
        public string Asunto { get; set; }
        public string Cuerpo { get; set; }
        public long NumeroPoliza { get; set; }
        public string Email { get; set; }
        public string Cliente { get; set; }
        public string UrlDocumento { get; set; }
    }
    public class CorreoParam
    {
        public int IdAuto { get; set; }
        public string UrlAdjunto { get; set; }

    }

    public class CorreoConfig
    {
        public string Mail { get; set; }
        public string Port { get; set; }
        public string Host { get; set; }
        public string Password { get; set; }
        public string Subject { get; set; }
    }
}
