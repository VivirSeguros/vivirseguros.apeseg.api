using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivirSeguros.CRM.Application.Services.SAC.FONDOSMAX.Queries
{
    public class GetProductoFondosmaxViewModel 
    {
        public string NroPoliza { get; set; }
        public string NroDocumento { get; set; }
        public string Portafolio { get; set; }
        public string InicioVigencia { get; set; }
        public string FinVigencia { get; set; }
        public string PeriodoDiferimento { get; set; }
        public string InicioPago { get; set; }
        public string PlazoVigencia { get; set; }
        public string MontoRenta { get; set; }
        public string MontoPrimaUnica { get; set; }
        public string MontoInversion { get; set; }
        public string PeriocidadPago { get; set; }
        public string Moneda { get; set; }
        public string CostoAdquisicion { get; set; }
        public string ComisionAdministracion { get; set; }
        public string FechaEmision { get; set; }
    }
}
