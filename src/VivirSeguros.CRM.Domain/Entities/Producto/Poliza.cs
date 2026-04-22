using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivirSeguros.CRM.Domain.Entities.Producto
{
    public class Poliza
    {
        public string NroPoliza { get; set; }
        public string RutaPoliza { get; set; }
        public Int32 NroEndoso { get; set; }
        public string Portafolio { get; set; }
        public decimal Plazo { get; set; }
        public string Moneda { get; set; }
        public decimal Prima { get; set; }
        public decimal PrimaInversion { get; set; }
        public string FechaEmision { get; set; }  // DateTime
        public string FechaAcreditacion { get; set; }  // DateTime
        public decimal ValorCuotaInicial { get; set; }
        public string FechaVCFM { get; set; }
        public string TipoFM { get; set; }
        public string SaldoFM { get; set; }
        public List<EstadoCuenta> EstadoCuenta { get; set; }
        public Asegurado Asegurado { get; set; }
        public List<Beneficiario> Beneficiarios { get; set; }
        public List<ValorCuot> ValorCuota { get; set; }
        public List<IndiceAV> ValorIndice { get; set; }

    }
}
