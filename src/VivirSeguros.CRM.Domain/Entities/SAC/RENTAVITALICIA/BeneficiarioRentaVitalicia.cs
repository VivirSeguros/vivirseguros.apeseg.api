using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivirSeguros.CRM.Domain.Entities.SAC.RENTAVITALICIA
{
    public class BeneficiarioRentaVitalicia
    {
        public string NroPoliza { get; set; }
        public string NroDocumento { get; set; }
        public string Nombre1 { get; set; }
        public string Nombre2 { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string FechaNacimiento { get; set; }
        public string Direccion { get; set; }
        public string Departamento { get; set; }
        public string Provincia { get; set; }
        public string Distrito { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }
        public string Estado { get; set; }
        public string MontoPrima { get; set; }
        public string Estudiante { get; set; }
    }
}
