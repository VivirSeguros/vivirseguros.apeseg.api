using System;
using System.Collections.Generic;

namespace VidaCamara.Domain.Services.Entidades.Apeseg
{
    #region PARAMETROS API (Lo que recibe tu Controller)

    public class ApesegLog
    {
        public string Tipo { get; set; }
        public string Nro { get; set; }
        public int Digito { get; set; }
        public string Err { get; set; }
        public string Envia { get; set; }
        public string Recibe { get; set; }
        public string User { get; set; }
        public string Proveedor { get; set; }
        public string Canal { get; set; }
        public string PuntoVenta { get; set; }
        public string IpCliente { get; set; }
        public string TramaEnvio { get; set; }
        public string TramaRespuesta { get; set; }
    }
    public class RegistroSOATRequest
    {
        public string CodigoAseguradora { get; set; }
        public string PolizaCertificado { get; set; }
        public string FechaInicioVigencia { get; set; }
        public string FechaFinVigencia { get; set; }
        public string CodigoTipoPersona { get; set; }
        public string NombreContratante { get; set; }
        public string CodigoTipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string PlacaVehiculo { get; set; }
        public string CodigoUsoVehiculo { get; set; }
        public string CodigoClaseVehiculo { get; set; }
        public string PaisPlaca { get; set; }
        public string FechaIngreso { get; set; }
        public string CodigoUbigeo { get; set; }
        public string NumeroSerieMotor { get; set; }
        public string FechaControlPolicial { get; set; }
        public string TipoCertificado { get; set; }
        public string TelefonoContacto { get; set; }
        public string CorreoContacto { get; set; }
        public string Marca { get; set; }
        public string NumeroAsientos { get; set; }
        public string ModeloVehiculo { get; set; }
        public string Proveedor { get; set; }
        public string Canal { get; set; }
        public string PuntoVenta { get; set; }
        public string IpCliente { get; set; }
        public string UsuarioRegistro { get; set; }

    }

    // Modificar hereda de Registro porque usa los mismos campos + el Dígito
    public class ModificarSOATRequest 
    {
        public string CodigoAseguradora { get; set; }
        public string PolizaCertificado { get; set; }
        public string FechaInicioVigencia { get; set; }
        public string FechaFinVigencia { get; set; }
        public string CodigoTipoPersona { get; set; }
        public string NombreContratante { get; set; }
        public string CodigoTipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string PlacaVehiculo { get; set; }
        public string CodigoUsoVehiculo { get; set; }
        public string CodigoClaseVehiculo { get; set; }
        public string PaisPlaca { get; set; }
        public string FechaActualizacion { get; set; }
        public string CodigoUbigeo { get; set; }
        public string NumeroSerieMotor { get; set; }
        public string NumeroSerieChasis { get; set; }
        public string FechaControlPolicial { get; set; }
        public string TipoCertificado { get; set; }
        public string TelefonoContacto { get; set; }
        public string CorreoContacto { get; set; }
        public string Marca { get; set; }
        public string NumeroAsientos { get; set; }
        public string ModeloVehiculo { get; set; }
        public string DigitoVerificador { get; set; }

        public string Proveedor { get; set; }
        public string Canal { get; set; }
        public string PuntoVenta { get; set; }
        public string IpCliente { get; set; }

        //public string FechaRegistro { get; set; }
        public string UsuarioRegistro { get; set; }

    }

    public class AnulacionSOATRequest
    {
        public string CodigoAseguradora { get; set; }
        public string PolizaCertificado { get; set; }
        public string DigitoVerificador { get; set; }
        public string CodigoTipoAnulacion { get; set; }
        public string FechaAnulacion { get; set; }
        public string Proveedor { get; set; }
        public string Canal { get; set; }
        public string PuntoVenta { get; set; }
        public string IpCliente { get; set; }
        public string UsuarioRegistro { get; set; }
        //public string FechaRegistro { get; set; }
    }
    public class ConsultaSOATRequest
    {
        public string placa { get; set; }
        //public string subscriptionKey { get; set; } = null;
        public string Proveedor { get; set; }
        public string Canal { get; set; }
        public string PuntoVenta { get; set; }
        public string IpCliente { get; set; }
        public string UsuarioRegistro { get; set; }
    }
    #endregion

    #region PARAMETROS PARA EL SERVICIO EXTERNO (Lo que espera APESEG)

    public class RegistrarParam
    {
        public string CodigoAseguradora { get; set; }
        public string PolizaCertificado { get; set; }
        public string FechaInicioVigencia { get; set; }
        public string FechaFinVigencia { get; set; }
        public string CodigoTipoPersona { get; set; }
        public string NombreContratante { get; set; }
        public string CodigoTipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string PlacaVehiculo { get; set; }
        public string CodigoUsoVehiculo { get; set; }
        public string CodigoClaseVehiculo { get; set; }
        public string PaisPlaca { get; set; }
        public string CodigoUbigeo { get; set; }
        public string NumeroSerieMotor { get; set; }
        public string FechaControlPolicial { get; set; }
        public string TipoCertificado { get; set; }
        public string TelefonoContacto { get; set; }
        public string CorreoContacto { get; set; }
        public string Marca { get; set; }
        public string NumeroAsientos { get; set; }
        public string ModeloVehiculo { get; set; }

        public string UsuarioCreacion { get; set; }
        public string FechaIngreso { get; set; }
    }

    public class ModificarParam
    {
        public string CodigoAseguradora { get; set; }
        public string PolizaCertificado { get; set; }
        public string FechaInicioVigencia { get; set; }
        public string FechaFinVigencia { get; set; }
        public string CodigoTipoPersona { get; set; }
        public string NombreContratante { get; set; }
        public string CodigoTipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string PlacaVehiculo { get; set; }
        public string CodigoUsoVehiculo { get; set; }
        public string CodigoClaseVehiculo { get; set; }
        public string PaisPlaca { get; set; }
        public string CodigoUbigeo { get; set; }
        public string NumeroSerieMotor { get; set; }
        public string FechaControlPolicial { get; set; }
        public string TipoCertificado { get; set; }
        public string TelefonoContacto { get; set; }
        public string CorreoContacto { get; set; }
        public string Marca { get; set; }
        public string NumeroAsientos { get; set; }
        public string ModeloVehiculo { get; set; }
        

        public string NumeroSerieChasis { get; set; }
        public string DigitoVerificador { get; set; }
        public string FechaActualizacion { get; set; }
        public string UsuarioModificacion { get; set; }
    }

    public class AnularParam
    {
        public string codigoAseguradora { get; set; }
        public string polizaCertificado { get; set; }
        public string digitoVerificador { get; set; }
        public string codigoTipoAnulacion { get; set; }
        public string fechaAnulacion { get; set; }
        public string usuarioModificacion { get; set; }
    }

    public class ConsultarParam
    {
        public string placa { get; set; }
    }
    #endregion

    #region RESPONSE API

    public class RegistrarResponse
    {
        public bool OperacionExitosa { get; set; }
        public string MensajeOperacion { get; set; }
        public List<string> CodigoError { get; set; }
        public List<string> MensajeError { get; set; }
        public int DigitoVerificador { get; set; }
    }

    public class ModificarResponse
    {
        public bool OperacionExitosa { get; set; }
        public string MensajeOperacion { get; set; }
        public List<string> CodigoError { get; set; }
        public List<string> MensajeError { get; set; }

    }

    public class AnularResponse
    {
        public bool OperacionExitosa { get; set; }
        public string MensajeOperacion { get; set; }
        public List<string> CodigoError { get; set; }
        public List<string> MensajeError { get; set; }
    }

    public class ConsultarResponse
    {
        public bool OperacionExitosa { get; set; }
        public string MensajeOperacion { get; set; }
        public List<string> CodigoError { get; set; }
        public List<Consulta> Certificados { get; set; }
    }

    public class Consulta
    {
        public string NombreCompania { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public DateTime? FechaFinVigencia { get; set; }
        public DateTime? FechaInicioVigencia { get; set; }
        public DateTime? FechaInicioD { get; set; }
        public DateTime? FechaFinD { get; set; }
        public string Placa { get; set; }
        public string Certificado { get; set; }
        public string NombreUsoVehiculo { get; set; }
        public string NombreClaseVehiculo { get; set; }
        public string Estado { get; set; }
        public string CodigoSBSAseguradora { get; set; }
        public string CodigoUnicoPoliza { get; set; }
        public string EstaAnulado { get; set; }
    }
    #endregion

    public class ConfigAPESEG
    {
        public int BatchSize { get; set; }
        public int DelayInSeconds { get; set; }
    }

    public class ApesegErrorCatalogo
    {
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
    }
}