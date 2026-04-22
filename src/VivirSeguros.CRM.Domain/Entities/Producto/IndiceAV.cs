using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivirSeguros.CRM.Domain.Entities.Producto
{
    public class IndiceAV
    {
        public DateTime FechaIndice { get; set; }
        public decimal ValorIndiceHoy { get; set; }
        public decimal PrimaActual { get; set; }
    }
}
