using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivirSeguros.CRM.Domain.Entities.Producto
{
    public class BeneficiarioSepelio
    {
        public string Nombres { get; set; }
        public string ApPaterno { get; set; }
        public string ApMaterno { get; set; }
        public string NroDocumento { get; set; }
        public string Parentesco { get; set; }
        public string FecNacimiento { get; set; }       
        public string Genero { get; set; }         
        public string Direccion { get; set; }
        public string Distrito { get; set; }
        public string Provincia { get; set; }
        public string Departamento { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        
    }
}
