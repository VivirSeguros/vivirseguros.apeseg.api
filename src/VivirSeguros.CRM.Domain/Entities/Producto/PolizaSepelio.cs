using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivirSeguros.CRM.Domain.Entities.Producto
{
    public class PolizaSepelio
    {
        public string NroPoliza { get; set; }
        public string Poliza { get; set; }
        public string PolizaEstado { get; set; }
        public string FechaInicio { get; set; }  // DateTime
        public string NroAsegurado { get; set; }
        public decimal CoberturaSumaAsegurada { get; set; }
        public decimal IndemnizacionMuerte { get; set; }
        public decimal BeneficioEstudiantil { get; set; }
        public decimal BeneficioLuto { get; set; }
        public string ProgramaBeneficio { get; set; }
        public string NumeroTarjeta { get; set; }
        public string Moneda { get; set; }
        public string RutaPoliza { get; set; }
        public string RutaCambioTarjeta { get; set; }
        public decimal Prima { get; set; }
        public decimal PrimaIgv { get; set; }
        public AseguradoSepelio Asegurado { get; set; }
        public List<BeneficiarioSepelio> Beneficiarios { get; set; }
        public List<ValorCuotSepelio> ValorCuota { get; set; }
    }
}
