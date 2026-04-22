using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivirSeguros.CRM.Domain.Entities.Producto
{
    public class AseguradoPRV
    {
        public int idAsegurado { get; set; }
        public string numeroDocumento { get; set; }
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public string nombres { get; set; }
        public string fechaNacimiento { get; set; }
        public string ubigeo { get; set; }
        public string direccion { get; set; }
        public string referencia { get; set; }
        public string telefono { get; set; }
        public string celular { get; set; }
        public string correoElectronico { get; set; }
        public string invalidez { get; set; }
        public string idDocumento { get; set; }
        public string idGenero { get; set; }
        public string idEstadoCivil { get; set; }
        public string fechaNidNacionalidadacimiento { get; set; }
        public string idPaisResidencia { get; set; }
        public string idDepartamento { get; set; }
        public string idProvincia { get; set; }
        public string idDistrito { get; set; }
        public string aseguradoContratante { get; set; }

    }
}
