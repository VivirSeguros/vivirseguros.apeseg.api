using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivirSeguros.CRM.Domain.Entities.Producto
{
    public class Asegurado
    {
        public string TipoDocumento { get; set; }
        public string NroDocumento { get; set; }
        public string ApPaterno { get; set; }
        public string ApMaterno { get; set; }
        public string Nombre1 { get; set; }
        public string Nombre2 { get; set; }
        public string FecNacimiento { get; set; }  //DateTime
        public string Genero { get; set; }
        public string Nacionalidad { get; set; }
        public string PaisResidencia { get; set; }
        public string Ubigeo { get; set; }
        public string Departamento { get; set; }
        public string Provincia { get; set; }
        public string Distrito { get; set; }
        public string Direccion { get; set; }
        public string Teléfono { get; set; }
        public string Correo { get; set; }
    }
}
