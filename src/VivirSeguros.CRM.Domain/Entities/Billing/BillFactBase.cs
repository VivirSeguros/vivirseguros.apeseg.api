using Newtonsoft.Json;
namespace VivirSeguros.CRM.Domain.Entities.Billing
{
    public class BillFactBase
    {
        public BoletaBase boleta { get; set; }
        public FacturaBase factura { get; set; }
        [JsonIgnore]
        public string tipoComprobante { get; set; }
        [JsonIgnore]
        public int idEstado { get; set; }
        [JsonIgnore]
        public int idProducto { get; set; }
    }
}
