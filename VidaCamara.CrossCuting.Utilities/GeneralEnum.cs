using System;
using System.Collections.Generic;
using System.Text;

namespace VidaCamara.CrossCuting.Utilities
{
    public static class GeneralEnum 
    {
        public static string FechaDefecto { get { return "9999-12-31"; } }

        public enum Documento
        {
            CondicionParticular = 1,
            Resumen = 2,
            ConvenioPago = 3,
            Solicitud = 4
        }
    }

    public static class LogTransacEnum
    {
        public static int PasoInicial { get { return 1; } }
        public static int PasoFinal { get { return 2; } }
        public static int PasoError { get { return -1; } }
    }

    public static class TipoLogTransac
    {
        public static int CreacionDocumento {  get { return 1; } }
        public static int EnvioCorreo { get { return 2; } }
        public static int ErrorApi { get { return 3; } }
    }

    public static class TipoConsultaVenta
    {
        public static int Placa { get { return 1; } }
        public static int DocumentoIdentidad { get { return 2; } }
    }

    public static class EstadoHistoriaInfocore
    {
        public static int Correcto { get { return 8; } }
        public static int Error { get { return 9; } }
        public static int SinDatos { get { return 10; } }
        public static int Existente { get { return 11; } }
        public static int ErrorInfoCore { get { return 12; } }
    }

    
}
