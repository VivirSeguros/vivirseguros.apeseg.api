using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivirSeguros.CRM.Domain.Entities.Producto
{
    public class SOAT
    {
		public string NroPoliza { get; set; }
		public string RutaPoliza { get; set; }
		public Int32 DiasVencimiento { get; set; }
		public string MensajeRenueva { get; set; }
		public string Placa { get; set; }
		public string InicioVigencia { get; set; }  //DateTime
		public string FinVigencia { get; set; }    //DateTime
		public string Marca { get; set; }
		public string Modelo { get; set; }
		public string Clase { get; set; }
		public string Uso { get; set; }
		public string Serie { get; set; }
		public string FechaEmision { get; set; }  //DateTime
		public string HoraEmision { get; set; }
		public Decimal Prima { get; set; }
		public int AnioFab { get; set; }
		public int NroAsientos { get; set; }
		public int idPersona { get; set; }
		public int idAuto { get; set; }
		public string NumeroTarjeta { get; set; }
		public string RutaCambioTarjeta { get; set; }
		public Asegurado Asegurado { get; set; }
	}
}
