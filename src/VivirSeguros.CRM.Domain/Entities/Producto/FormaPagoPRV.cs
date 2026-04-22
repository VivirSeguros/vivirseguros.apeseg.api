using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivirSeguros.CRM.Domain.Entities.Producto
{
    public class FormaPagoPRV
    {
        public string MONEDA { get; set; }
        public string numeroCuenta { get; set; }
        public string fechaAbono { get; set; }
        public string FECHAFIRMA { get; set; }
        public string TIPOCUENTA { get; set; }

    }
}
