using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivirSeguros.CRM.Domain.Entities.SAC.SOAT
{
     public class Polizasoat
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
