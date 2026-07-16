using System;
using System.Collections.Generic;
using System.Text;

namespace VidaCamara.Domain.Services.Entidades
{
    public class ResponseTrx
    {
        public int Codigo { get; set; }
        public string Mensaje { get; set; }
        public int CodigoGenerado { get; set; }
        public int CodigoGenerado1 { get; set; }
        public dynamic ValorGenerado { get; set; }
        public dynamic ValorGenerado1 { get; set; }
        public dynamic ValorGenerado2 { get; set; }
    }
}
