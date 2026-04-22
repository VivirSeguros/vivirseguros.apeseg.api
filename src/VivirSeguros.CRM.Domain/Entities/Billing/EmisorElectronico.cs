using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivirSeguros.CRM.Domain.Entities.Billing
{
    public class EmisorElectronico
    {
        public string tipoDocId { get; set; }
        public string numeroDocId { get; set; }
        public string nombreComercial { get; set; }
        public string razonSocial { get; set; }
        public string ubigeo { get; set; }
        public string direccion { get; set; }
        public string codigoPais { get; set; }
        public string telefono { get; set; }
        public string correoElectronico { get; set; }
        public string codigoAsigSUNAT { get; set; }
    }
}
