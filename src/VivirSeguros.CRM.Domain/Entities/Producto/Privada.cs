using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivirSeguros.CRM.Domain.Entities.Producto
{
    public class Privada
    {
        public string numeroPoliza { get; set; }
        public int idPoliza { get; set; }
        public int idModalidad { get; set; }
        public string MONEDA { get; set; }
        public decimal prcTasaAjusRentAnual { get; set; }
        public decimal prcDevPrima { get; set; }
        public decimal periodoVigencia { get; set; }
        public decimal periodoDiferido { get; set; }
        public decimal periodoGarantizado { get; set; }
        public decimal primerTramo { get; set; }
        public decimal segundoTramo { get; set; }
        public string gratificacion { get; set; }
        public int idMoneda { get; set; }
        public decimal prima { get; set; }
        public decimal tasaVenta { get; set; }
        public string fechadevengue { get; set; }  // Datetime
        public decimal rentaTitular { get; set; }
        public AseguradoPRV AseguradoPRV { get; set; }
        public List<BeneficiarioPRV> BeneficiarioPRV { get; set; }
        public FormaPagoPRV FormaPagoPRV { get; set; }

    }
}
