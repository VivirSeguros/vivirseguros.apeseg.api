using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivirSeguros.CRM.Domain.Entities.Billing
{
    public class Detalle
    {
        public string numeroItem { get; set; }
        public string codigoProducto { get; set; }
        public string descripcionProducto { get; set; }
        public string cantidadItems { get; set; }
        public string unidad { get; set; }
        public string valorUnitario { get; set; }
        public string precioVentaUnitario { get; set; }
        public List<TotalImpuestoDetalle> totalImpuestos { get; set; }
        public string valorVenta { get; set; }
        public string codProductoSunat { get; set; }
        public string montoTotalImpuestos { get; set; }
    }
}
