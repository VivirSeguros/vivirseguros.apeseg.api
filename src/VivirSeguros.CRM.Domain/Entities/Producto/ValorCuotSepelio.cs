using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivirSeguros.CRM.Domain.Entities.Producto
{
    public class ValorCuotSepelio
    {
        public string Nombre1 { get; set; }
        public string Nombre2 { get; set; }
        public string ApPaterno { get; set; }
        public string ApMaterno { get; set; }
        public string FechaPago { get; set; }
        public decimal Prima { get; set; }
        public string EstadoPago { get; set; }
        //public int NroCuota { get; set; }
        public string Cuota { get; set; }
    }
}
