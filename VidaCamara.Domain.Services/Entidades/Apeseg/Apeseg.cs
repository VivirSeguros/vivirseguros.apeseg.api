using System;
using System.Collections.Generic;

namespace VidaCamara.Domain.Services.Entidades.Apeseg
{
    public class ApesegParam
    {
        public int idAuto { get; set; }
        public string Accion { get; set; }
        public int Digito { get; set; }
        public string TipoAnulacion { get; set; }
    }

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
        public string FechaIngreso { get; set; }
        public string CodigoUbigeo { get; set; }
        public string NumeroSerieMotor { get; set; }
        public string NumeroSerieChasis { get; set; }
        public string FechaControlPolicial { get; set; }
        public string TipoCertificado { get; set; }
        public string TelefonoContacto { get; set; }
        public string CorreoContacto { get; set; }
        public string UsuarioCreacion { get; set; }
        public string Marca { get; set; }
        public string NumeroAsientos { get; set; }
        public string ModeloVehiculo { get; set; }
    }

    public class Registrar
    {
        public bool OperacionExitosa { get; set; }
        public string MensajeOperacion { get; set; }
        public List<string> CodigoError { get; set; }
        public int DigitoVerificador { get; set; }
    }

    public class ModificarParam
    {
        public string CodigoAseguradora { get; set; }
        public string PolizaCertificado { get; set; }
        public string DigitoVerificador { get; set; }
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
        public string UsuarioModificacion { get; set; }
        public string Marca { get; set; }
        public string NumeroAsientos { get; set; }
        public string ModeloVehiculo { get; set; }
    }

    public class Modificar
    {
        public bool OperacionExitosa { get; set; }
        public string MensajeOperacion { get; set; }
        public List<string> CodigoError { get; set; }
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

    public class Anular
    {
        public bool OperacionExitosa { get; set; }
        public string MensajeOperacion { get; set; }
        public List<string> CodigoError { get; set; }
    }

    public class Consultar
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
        public DateTime? FechaFinVigencia { get; set; }
        public string FechaFin { get; set; }
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


    public class ConsultaMasivaRequest
    {
        public string IdUsuario { get; set; }
        public string Usuario { get; set; } = "";
        public string ArchivoNombre { get; set; }
        public int idProceso { get; set; }
        public string Placa { get; set; }
        public int CantidadTotal { get; set; }
        public string Mensaje { get; set; } = "";
        public string Estado { get; set; } = "";

    }

    public class ConsultaMasivaResponse
    {
        public int idProceso { get; set; }
        public string Mensaje { get; set; }

    }

    public class PlacaAPESEG
    {
        public string Placa { get; set; }

    }


    public class ApesegConsultaPlacaMasivo
    {
        public int Id { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime? FechaModificado { get; set; }
        public string UsuarioRegistro { get; set; }
        public string UsuarioModifica { get; set; }
        public string Estado { get; set; }
        public string ArchivoNombre { get; set; }
        public decimal Avance { get; set; }
        public string Observacion { get; set; }
        public int PlacasTotal { get; set; }
        public DateTime? FechaTermino { get; set; }
    }

    public class ApesegConsultaPlacaMasivoDetalle
    {
        public int Id { get; set; }
        public int IdProceso { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime? FechaModificado { get; set; }
        public string UsuarioRegistro { get; set; }
        public string UsuarioModifica { get; set; }
        public string NombreCompania { get; set; }
        public string FechaInicio { get; set; }
        public DateTime? FechaFinVigencia { get; set; }
        public string FechaFin { get; set; }
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
        public string Revisado { get; set; }
        public int Orden { get; set; }
    }

    public class ConfigAPESEG
    {
        public int BatchSize { get; set; }
        public int DelayInSeconds { get; set; }

    }

    public class EnvioCarritoResponse
    {
        public int idProceso { get; set; }
        public string Mensaje { get; set; }
        public List<string> Placas { get; set; }


        public EnvioCarritoResponse()
        {
            Placas = new List<string>();
        }
    }

    public class EnvioCarritoRequest
    {
        public int Toque { get; set; }

    }

    public class ActualizaCarritoRequest
    {
        public List<PlacaCarritoRequest> Placas { get; set; }


        public ActualizaCarritoRequest()
        {
            Placas = new List<PlacaCarritoRequest>();
        }

    }

    public class PlacaCarritoRequest
    {
        public string Placa { get; set; }
        public int Intento { get; set; }
        public bool Comprado { get; set; }
        public string FechaInicioApeseg { get; set; }
        public string FechaFinApeseg { get; set; }
        public string NombreCompania { get; set; }
    }

    public class NotificaCarritoResponse
    {
        public int idProceso { get; set; }
        public string Mensaje { get; set; }
        public List<NotificacionPlaca> Placas { get; set; }


        public NotificaCarritoResponse()
        {
            Placas = new List<NotificacionPlaca>();
        }
    }

    public class NotificacionPlaca
    {
        public string Placa { get; set; }
        public bool CorreoEnviado { get; set; }
        public bool WhatsAppEnviado { get; set; }
    }

}