using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivirSeguros.CRM.Domain.Entities.SAC.RENTAMAX
{
    public class BeneficiarioRentamax
    {
        public string NroPoliza { get; set; }
        public string NroDocumento { get; set; }
        public string Nombre1 { get; set; }
        public string Nombre2 { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string FechaNacimiento { get; set; }
        public string CodigoInvalidez { get; set; }
        public string Parentesco { get; set; }
        public string PensionBeneficiario { get; set; }
        public string FechaInicioVigencia { get; set; }
        public string FechaFinVigencia { get; set; }
    }
}
