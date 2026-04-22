using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivirSeguros.CRM.Domain.Entities.SAC.SOAT
{
    public class VehiculoSoat
    {
        public string NroPoliza { get; set; }
        public string NroDocumento { get; set; }
        public string Placa { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Clase { get; set; }
        public string Carroceria { get; set; }
        public string Uso { get; set; }
        public string AnioFabricacion { get; set; }
        public string Asientos { get; set; }
        public string Serie { get; set; }
    }
}
