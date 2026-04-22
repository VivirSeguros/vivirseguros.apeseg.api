using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VivirSeguros.CRM.Domain.Entities.Producto;

namespace VivirSeguros.CRM.Application.Services.Producto.Queries
{
    public class GetProductoViewModel
    {
        //public string NroDoc { get; set; }
        public Int32 IdProducto { get; set; }
        public string NombreProducto { get; set; }      
        public List<Poliza> PolizaFM { get; set; }
        public List<SOAT> PolizaSOAT { get; set; }
        public List<Vitalicia> PolizaRV { get; set; }
        public List<Privada> PolizaPRV { get; set; }
        public List<PolizaSepelio> PolizaSepelio { get; set; }
    }
}
