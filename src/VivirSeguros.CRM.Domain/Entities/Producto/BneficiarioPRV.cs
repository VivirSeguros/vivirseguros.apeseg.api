using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivirSeguros.CRM.Domain.Entities.Producto
{
    public class BeneficiarioPRV
    {
        public int idBeneficiario { get; set; }
        public string nombres { get; set; }
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public string idDocumento { get; set; }
        public string numeroDocumento { get; set; }
        public string fechaNacimiento { get; set; }
        public string idGenero { get; set; }
        public string idParentesco { get; set; }
        public string invalido { get; set; }
        public int edadSalida { get; set; }
        public decimal porcentajePension { get; set; }
        public string nombre { get; set; }
        public string idTipoBeneficiario { get; set; }
        public string tipo { get; set; }

    }
}
