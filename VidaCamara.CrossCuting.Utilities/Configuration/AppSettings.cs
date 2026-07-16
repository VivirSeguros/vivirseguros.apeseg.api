using System;
using System.Collections.Generic;
using System.Text;

namespace VidaCamara.CrossCuting.Utilities.Configuration
{
   public class AppSettings
    {
        public int Activo { get; set; }
        public string Ambiente { get; set; }
        public string Secret { get; set; }
        public string Site { get; set; }
        public string ExpireToken { get; set; }
        public string ConnectionString { get; set; }
        public string ConnectionStringRI { get; set; }
        public string ConnectionStringSepelio { get; set; }
        public string ConnectionStringCRM { get; set; }
        public string AADInstance { get; set; }
        public string Tenant { get; set; }
        public string Resource { get; set; }
        public string BaseAddressAPI { get; set; }
        public string BaseAddressConsulta { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string SubscriptionKey { get; set; }
        public string PathHelperExe { get; set; }
        public string Izipay { get; set; }
        public string IziUsuario { get; set; }
        public string IziClave { get; set; }
        public string IziCorreo { get; set; }
        public string IziCorreoActivo { get; set; }
        public int HoraToken { get; set; }
        public string UrlInsertSepelioPolizaAsync { get; set; }
        public string UrlGenerarDocumentoAsync { get; set; }
        public string UrlCuadroPolizaAsync { get; set; }
        public string UrlEnviarCorreoAsync { get; set; }
        public string UrlEnviarWhatsAppAsync { get; set; }
        public string TipoCorreo { get; set; }
        public string TipoCorreoAnulacion { get; set; }
        public string TipoCorreoAnulacionSinAdi { get; set; }
        public string TipoWhatsapp { get; set; }
        public string TipoCorreoT1 { get; set; }
        public string TipoCorreoT2 { get; set; }
        public string TipoCorreoT3 { get; set; }
        public string TipoWhatsappT1 { get; set; }
        public string TipoWhatsappT2 { get; set; }
        public string TipoWhatsappT3 { get; set; }
        public string App_BaseUrl { get; set; }
    }
}
