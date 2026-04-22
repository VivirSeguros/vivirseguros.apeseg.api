using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivirSeguros.CRM.Application.Services.SAC.RENTAMAX.Queries
{
    public class GetProductoRentamaxViewModel
    {
        public string NroPoliza { get; set; }
        public string NroDocumento { get; set; }
        public string FechaInicioVigencia { get; set; }
        public string FechaFinVigencia { get; set; }
        public string MontoPrima { get; set; }
        public string TasaVenta { get; set; }
        public string Moneda { get; set; }
        public string TasaAjustePago { get; set; }
        public string MontoRentaBase { get; set; }
        public string PeridiocidadPago { get; set; }
        public string FechaDevengue { get; set; }
        public string DiferenciamientoPago { get; set; }
        public string FechaInicioPagoRenta { get; set; }
        public string FechaInicioPagoRentaDiferida { get; set; }
        public string FechaFinPagoRentaDiferida { get; set; }
        public string NroMesesPeriodoGarantizado { get; set; }
        public string FechaInicioVigenciaPG { get; set; }
        public string FechaFinVigenciaPG { get; set; }
        public string TasaDescuentoPago { get; set; }
        public string AnticipoPorc { get; set; }
        public string TasaDescuentoAnticipo { get; set; }
        public string FechaInicioVigenciaPrimerTramo { get; set; }
        public string FechaFinVigenciaPrimerTramo { get; set; }
        public string Tramo2PorcTramo1 { get; set; }
        public string CoberturaSepelio { get; set; }
        public string FechaFinVigenciaCoberturaSepelio { get; set; }
        public string MontoDevolucionPrima { get; set; }
        public string FechaDevolucionPrima { get; set; }
        public string ClausulaRescate { get; set; }
        public string CoberturaPagoBeneficiarios { get; set; }
        public string MontoRenta { get; set; }
        public string PrimaUnica { get; set; }
    }
}
