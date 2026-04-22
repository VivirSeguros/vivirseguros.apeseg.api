using System.Collections.Generic;

namespace VivirSeguros.CRM.Domain.Entities.Billing
{
    public class CabeceraBase
    {
        public Gravadas gravadas { get; set; }
        public List<TotalImpuesto> totalImpuestos { get; set; }
        public string importeTotal { get; set; }
        public string tipoOperacion { get; set; }
        public Leyenda leyenda { get; set; }
        public string montoTotalImpuestos { get; set; }
    }
}
