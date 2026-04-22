using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivirSeguros.CRM.Domain.Entities
{
    public class Bill_Fact
    {
        public int IdTipoComprobante { get; set; }
        public string Descripcion { get; set; }
        public int IdTipoDocumento { get; set; }
        public string Numeracion { get; set; }
        public string FechaEmision { get; set; }
        public string TipoMoneda { get; set; }
        public string FechaVencimiento { get; set; }
        public string NroDocEmi { get; set; }
        public string RazonSocialEmi { get; set; }
        public string DireccionEmi { get; set; }
        public string TelefonoEmi { get; set; }
        public string CodigoSunatEmi { get; set; }
        public string TipoDocIdRec { get; set; }
        public string NumeroDocIdRec { get; set; }
        public string NombreCliente { get; set; }
        public string DireccionRec { get; set; }
        public string EmailRec { get; set; }
        public string CodOperacion { get; set; }
        public string Prima { get; set; }
        public string PrimaNeta { get; set; }
        public string Igv { get; set; }
        public string CodImpuesto { get; set; }
        public string TipoOperacion { get; set; }
        public string CodLeyenda { get; set; }
        public string DescripcionLeyenda { get; set; }
        public string NumeroItem { get; set; }
        public string NombreProducto { get; set; }
        public string CantidadItems { get; set; }
        public string Unidad { get; set; }
        public string TipoAfectacion { get; set; }
        public string PorcentajeIGV { get; set; }
        public string CodigoProductoSunat { get; set; }
        public string TituloAdicional { get; set; }
        public string ValorAdicional { get; set; }
        public string TituloAdicionalFac { get; set; }
        public string ValorAdicionalFac { get; set; }
        public string Estado { get; set; }
    }
}
