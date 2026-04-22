using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivirSeguros.CRM.Domain.Entities.Producto
{
    public class Vitalicia
    {
        public string Pension { get; set; }
        public string Moneda { get; set; }
        public string TipoRenta { get; set; }        
        public string Modalidad { get; set; }        
        public string num_cargas { get; set; }
        public string FLAGSOB { get; set; }
        public string Fecha_Devengue { get; set; }
        public string FEC_INGRESOSPP { get; set; }
        public string ANIOS_ESCALONADOS { get; set; }
        public decimal PensPORCENTAJE_MESES_ESCALONADOSion { get; set; }
        public string EstadoPOLIZA { get; set; }
        public string fec_vigencia { get; set; }
        public string fec_tervigencia { get; set; }
        public string AFP { get; set; }
        public decimal Mto_Prima { get; set; }
        public string ANIOS_Diferidos { get; set; }
        public string ANIOS_Garantizados { get; set; }
        public string prc_tasace { get; set; }
        public string prc_tasavta { get; set; }
        public string gls_fonoben { get; set; }
        public string gls_fono2 { get; set; }
        public string gls_fono3 { get; set; }
        public string gls_correoben { get; set; }
        public string gls_correo2 { get; set; }
        public string gls_correo3 { get; set; }
        public string cod_moneda { get; set; }
        public string FechaIniVigenciaGar { get; set; }
        public string FechaFinVigenciaGar { get; set; }
        public string cod_tippension { get; set; }
        public string NUM_POLIZA { get; set; }
        public string Fec_Inipencia { get; set; }
        public TitularRV TitularRV { get; set; }
        public TitularRV AseguradoRV { get; set; }
        public List<BeneficiarioRV> BeneficiarioRV { get; set; }
        public FormaPagoRV FormaPagoRV { get; set; }
    }
}
