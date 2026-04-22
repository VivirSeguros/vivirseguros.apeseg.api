using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace VivirSeguros.CRM.Domain.Entities.Producto
{
    public class Producto
    {
        public Int32 IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public List<Poliza> PolizaFM { get; set; }
        public List<SOAT> PolizaSOAT { get; set; }
        public List<Vitalicia> PolizaRV { get; set; }
        public List<Privada> PolizaPRV { get; set; }
        public List<PolizaSepelio> PolizaSepelio { get; set; }
    }
}
