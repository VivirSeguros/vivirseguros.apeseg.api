using System;
using System.Collections.Generic;
using System.Text;

namespace VidaCamara.Domain.Services.Entidades.Apeseg
{
    public class Pago
    {
        public string mode { get; set; }
        public string status { get; set; }
        public Respuesta answer { get; set; }

    }

    public class PagoParam
    {
        public int amount { get; set; }
        public string currency { get; set; }
        public Clipago customer { get; set; }
        public string orderId { get; set; }
        public Opcion transactionOptions { get; set; }

        public PagoParam()  {
            this.customer = new Clipago();
            this.transactionOptions = new Opcion();
        }

    }

    public class Clipago
    {
        public string email { get; set; }
        public Clipago() {
            this.email = "julio.guillen@vivirseguros.pe";
        }
        
    }

    public class Opcion
    {
        public Numero cardOptions { get; set; }
        public Opcion() {
            this.cardOptions = new Numero();
        }
    }

    public class Numero
    {
        public int installmentNumber { get; set; }
        public Numero()  {
            this.installmentNumber = 0;
        }
    }




    public class Respuesta
    {
        public string formToken { get; set; }
        public string user { get; set; }
        public int autoId { get; set; }
    }

    public class JSIzipay
    {        
        public clientAnswer clientAnswer { get; set; }
       
    }

    public class ordDetalle
    {
        public string orderId { get; set; }
    }

    public class clientAnswer
    {
        public string orderStatus { get; set; }
        public string shopId { get; set; }
        public string orderId { get; set; }
        public List<transaccion> transactions { get; set; }
        public ordDetalle orderDetails { get; set; }
    }

    public class transaccion
    {
       
        public string uuid { get; set; }
        public decimal amount { get; set; }
        public string currency { get; set; }
        public detalle transactionDetails { get; set; }
    }

    public class detalle
    {
        public string externalTransactionId { get; set; }
        public tarjeta cardDetails { get; set; }
    }
    public class tarjeta
    {
        public string effectiveBrand { get; set; }
    }

    public class OrdenPago
    {
        public int idOrdenPago { get; set; }
        public string Placa { get; set; }
        public decimal Prima { get; set; }
        public string NroDocumento { get; set; }
        public string JsonAuto { get; set; }
        public string FechaRegistro { get; set; }
        public string CodOrdenPago { get; set; }
        public string User { get; set; }
        public string Canal { get; set; }
    }

    public class OrdenPagoParam
    {
        public int idOrdenPago { get; set; }
        public string placa { get; set; }
        public int estado { get; set; }
        public string nroDoc { get; set; }
        public string fechaIni { get; set; }
        public string fechaFin { get; set; }
        public string ordenPago { get; set; }
    }


}


