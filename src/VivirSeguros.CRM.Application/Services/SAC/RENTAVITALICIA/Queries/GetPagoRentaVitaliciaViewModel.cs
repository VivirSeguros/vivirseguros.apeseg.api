using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivirSeguros.CRM.Application.Services.SAC.RENTAVITALICIA.Queries
{
    public class GetPagoRentaVitaliciaViewModel
    {
        public string NroPoliza { get; set; }
        public string NroDocumento { get; set; }
        public string MedioPago { get; set; }
        public string Sucursal { get; set; }
        public string Banco { get; set; }
        public string TipoCuenta { get; set; }
        public string NroCuenta { get; set; }
        public string CuentaCCI { get; set; }
    }
}
