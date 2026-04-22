using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivirSeguros.CRM.Domain.Entities.SAC.VIVEMAX
{
    public class ProductoVivemax
    {
        public string NroPoliza { get; set; }
        public string NroDocumento { get; set; }
        public string PlanProt { get; set; }
        public string Periodo { get; set; }
        public string InicioVigencia { get; set; }
        public string FinVigencia { get; set; }
        public string Prima { get; set; }
        public string SumaAsegurada { get; set; }
        public string FechaAnulacion { get; set; }

    }
}
