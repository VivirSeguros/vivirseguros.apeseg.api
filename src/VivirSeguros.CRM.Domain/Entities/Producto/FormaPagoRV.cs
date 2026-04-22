using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivirSeguros.CRM.Domain.Entities.Producto
{
    public class FormaPagoRV
    {
        public string TipoDoc { get; set; }
        public string NoDoc { get; set; }
        public string Nombre { get; set; }
        public string IniVigencia { get; set; }
        public string FinVigencia { get; set; }
        public string ViaPago { get; set; }
        public string Sucursal { get; set; }
        public string Banco { get; set; }
        public string Cuenta { get; set; }
        public string NoCuenta { get; set; }
        public string NoCuentaCCI { get; set; }
        public string Institucion { get; set; }
        public string ModalidadPago { get; set; }
        public decimal MontoPago { get; set; }
        public string PeriodoEfecto { get; set; }
    }
}
