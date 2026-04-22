using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivirSeguros.CRM.Domain.Entities.Billing
{
    public class FacturaBase
    {
        public IdentificadorDocumento IDE { get; set; }
        public EmisorElectronico EMI { get; set; }
        public ReceptorElectronico REC { get; set; }
        public CabeceraBase CAB { get; set; }
        public List<Detalle> DET { get; set; }
        public List<CampoAdicional> ADI { get; set; }
    }
}
