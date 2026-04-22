using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivirSeguros.CRM.Application.Services.SAC.VIVEMAX.Queries
{
   public class GetPagoVivemaxViewModel
    {
        public string NroPoliza { get; set; }
        public string NroDocumento { get; set; }
        public string MedioPago { get; set; }
        public string Banco { get; set; }
        public string PeriodoPagado { get; set; }
        public string MontoPagado { get; set; }
        public string Moneda { get; set; }
        public string FechaVencimiento { get; set; }
        public string FechaPago { get; set; }
    }
}
