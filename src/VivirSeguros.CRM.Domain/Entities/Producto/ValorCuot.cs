using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivirSeguros.CRM.Domain.Entities.Producto
{
    public class ValorCuot
    {
        public DateTime FechaCuota { get; set; }
        public decimal ValorCuotaHoy { get; set; }
        public decimal PrimaActual { get; set; }
    }
}
