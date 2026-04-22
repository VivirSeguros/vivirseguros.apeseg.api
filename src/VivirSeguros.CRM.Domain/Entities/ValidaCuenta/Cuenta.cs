using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivirSeguros.CRM.Domain.Entities.ValidaCuenta
{
    public class Cuenta
    {
        public Int32 EstadoLogin { get; set; }
        public string Mensaje { get; set; }
        public string Producto { get; set; }
        public string Poliza { get; set; }
        public string TipoDoc { get; set; }
        public string NumDoc { get; set; }
        public string ApPaterno { get; set; }
        public string ApMaterno { get; set; }
        public string Nombre1 { get; set; }
        public string Nombre2 { get; set; }
        public string RazonSocial { get; set; }
        public DateTime FecNac { get; set; }
        public string Estado { get; set; }
        public string TipoCliente { get; set; }
        public string Genero { get; set; }
        public string Correo { get; set; }
    }
}

