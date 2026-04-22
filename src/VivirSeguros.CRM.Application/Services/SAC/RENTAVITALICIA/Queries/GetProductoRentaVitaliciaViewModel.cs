using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivirSeguros.CRM.Application.Services.SAC.RENTAVITALICIA.Queries
{
     public class GetProductoRentaVitaliciaViewModel
    {
        public string NroPoliza { get; set; }
        public string NroDocumento { get; set; }
        public string NroCertificado { get; set; }
        public string TipoPension { get; set; }
        public string TipoMoneda { get; set; }
        public string TipoRenta { get; set; }
        public string NroBeneficiarios { get; set; }
        public string FechaDevengue { get; set; }
        public string InicioVigencia { get; set; }
        public string FinVigencia { get; set; }
        public string NombreAFP { get; set; }
        public string MontoPrima { get; set; }
        public string MontoPension { get; set; }
        public string AñosDiferidos { get; set; }
        public string AñosGrantizados { get; set; }
        public string VigenciaGarantizado { get; set; }
        public string PrimerPago { get; set; }
        public string Gratificacion { get; set; }
        public string MontoPagado { get; set; }
        public string FechaRegistro { get; set; }
        public string UsuarioRegistro { get; set; }
        public string FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
    }
}
 