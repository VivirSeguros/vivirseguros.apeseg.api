using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivirSeguros.CRM.Application.Services.SAC.SOAT.Queries
{
    public class GetPolizaSoatViewModel
    {
        public string NroPoliza { get; set; }
        public string NroDocumento { get; set; }
        public string Canal { get; set; }
        public string Prima { get; set; }
        public string InicioVigencia { get; set; }
        public string FinVigencia { get; set; }
        public string FechaEmision { get; set; }
        public string FechaAnulacion { get; set; }
    }
}
