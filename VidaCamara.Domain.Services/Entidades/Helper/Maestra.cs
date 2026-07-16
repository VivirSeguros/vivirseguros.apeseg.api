using System;
using System.Collections.Generic;
using System.Text;

namespace VidaCamara.Domain.Services.Entidades.Helper
{
    public class Maestra
    {
        public Int32 idMaestra { get; set; }
        public Int32 CodigoTabla { get; set; }
        public string Descripcion { get; set; }
        public string DescripcionCorta { get; set; }
        public Int32 Longitud { get; set; }
        public string Simbolo { get; set; }
        public string Valor1 { get; set; }
        public string Valor2 { get; set; }
        public string CodigoSBS { get; set; }
        public string CodigoApeseg { get; set; }
        public bool Soat { get; set; }
        public bool SoatDigital { get; set; }
        public string UsuarioCreacion { get; set; }
        public string FechaRegistro { get; set; }
        public string UsuarioModificacion { get; set; }
        public string FechaModificacion { get; set; }
    }
    public class MaestraParam
    {
        public Int32 idMaestra { get; set; }
    }

}
