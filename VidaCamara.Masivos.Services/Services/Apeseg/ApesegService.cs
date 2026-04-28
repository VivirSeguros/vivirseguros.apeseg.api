using AutoMapper;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VidaCamara.CrossCuting.Log.Contracts;
using VidaCamara.CrossCuting.Utilities;
using VidaCamara.CrossCuting.Utilities.Configuration;
using VidaCamara.Domain.Services.Entidades.Apeseg;
using VidaCamara.Domain.Services.Repositorios.Apeseg;
using VidaCamara.Domain.Services.Repositorios.Helper;

namespace VidaCamara.Masivos.Services.Services.Apeseg
{
    public class ApesegService : IApesegService
    {
        private readonly IApesegRepository apesegRepository;
        private ILoggerManager _logger;
        private IMapper _mapper;
        private readonly IHelperRepository _helperRepository;
        private readonly AppSettings _appSettings;

        private const string VENCIDO = "VENCIDO";

        public ApesegService(IApesegRepository apesegRepository, ILoggerManager logger, IMapper mapper, IHelperRepository helperRepository, IOptions<AppSettings> appSettings)
        {
            this.apesegRepository = apesegRepository;
            _logger = logger;
            _mapper = mapper;
            _helperRepository = helperRepository;
            _appSettings = appSettings.Value;
        }

        public async Task<Registrar> Apeseg_Send(ApesegParam param)
        {
            Registrar result = null;
            try
            {
                Registrar entitieApeseg = await apesegRepository.Apeseg_Send(param);

                result = _mapper.Map<Registrar>(entitieApeseg);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public async Task<Consultar> Consultar(string placa)
        {
            Consultar result = null;
            try
            {
                Consultar entitieApeseg = await apesegRepository.Consultar(placa);

                result = _mapper.Map<Consultar>(entitieApeseg);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public async Task<Consultar> Consultar(string placa, string subscriptionKey)
        {
            Consultar result = null;
            try
            {
                Consultar entitieApeseg = await apesegRepository.Consultar(placa, subscriptionKey);

                result = _mapper.Map<Consultar>(entitieApeseg);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        //public async Task<Consultar> Pago_Save(Respuesta param)
        //{
        //    Consultar result = null;
        //    try
        //    {
        //        Consultar entitieApeseg = await apesegRepository.Pago_Save(param);

        //        result = _mapper.Map<Consultar>(entitieApeseg);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return result;
        //}

        //public async Task<int> Orden_Pago(OrdenPago param)
        //{
        //    int result = 0;
        //    try
        //    {
        //        int entitieApeseg = await apesegRepository.Orden_Pago(param);

        //        //result = _mapper.Map<OrdenPago>(entitieApeseg);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return result;
        //}

        //public async Task<int> Validar_Pago(string Orden, decimal Monto)
        //{
        //    int result = 0;
        //    try
        //    {
        //        result = await apesegRepository.Validar_Pago(Orden, Monto);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return result;
        //}

        //public async Task<IEnumerable<OrdenPago>> GetOrden(OrdenPagoParam param)
        //{
        //    IEnumerable<OrdenPago> result;
        //    try
        //    {
        //        result = await apesegRepository.GetOrden(param);

        //        //result = _mapper.Map<OrdenPago>(entitieApeseg);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return result;
        //}

        //public async Task<ConsultaMasivaResponse> InsertaProcesoConsultaPlacasMasivo(ConsultaMasivaRequest consultaMasivaRequest)
        //{
        //    ConsultaMasivaResponse result;
        //    try
        //    {
        //        result = await apesegRepository.InsertaProcesoConsultaPlacasMasivo(consultaMasivaRequest);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return result;
        //}

        //public async Task<ConsultaMasivaResponse> ConsultaPlacasMasivo(ConsultaMasivaRequest consultaMasivaRequest, List<PlacaAPESEG> Placas)
        //{
        //    ConsultaMasivaResponse result = new ConsultaMasivaResponse();
        //    try
        //    {
        //        var dtLista = new DataTable("ApesegConsultaPlacaMasivoDetalle");
        //        // Agregar columnas al DataTable
        //        dtLista.Columns.Add("Id", typeof(int));
        //        dtLista.Columns.Add("IdProceso", typeof(int));
        //        dtLista.Columns.Add("FechaRegistro", typeof(DateTime));
        //        dtLista.Columns.Add("FechaModificado", typeof(DateTime));
        //        dtLista.Columns.Add("UsuarioRegistro", typeof(string));
        //        dtLista.Columns.Add("UsuarioModifica", typeof(string));
        //        dtLista.Columns.Add("NombreCompania", typeof(string));
        //        dtLista.Columns.Add("FechaInicio", typeof(string));
        //        dtLista.Columns.Add("FechaFinVigencia", typeof(DateTime));
        //        dtLista.Columns.Add("FechaFin", typeof(string));
        //        dtLista.Columns.Add("FechaInicioVigencia", typeof(DateTime));
        //        dtLista.Columns.Add("FechaInicioD", typeof(DateTime));
        //        dtLista.Columns.Add("FechaFinD", typeof(DateTime));
        //        dtLista.Columns.Add("Placa", typeof(string));
        //        dtLista.Columns.Add("Certificado", typeof(string));
        //        dtLista.Columns.Add("NombreUsoVehiculo", typeof(string));
        //        dtLista.Columns.Add("NombreClaseVehiculo", typeof(string));
        //        dtLista.Columns.Add("Estado", typeof(string));
        //        dtLista.Columns.Add("CodigoSBSAseguradora", typeof(string));
        //        dtLista.Columns.Add("CodigoUnicoPoliza", typeof(string));
        //        dtLista.Columns.Add("EstaAnulado", typeof(string));
        //        dtLista.Columns.Add("Orden", typeof(int));


        //        ConfigAPESEG configAPESEG = await apesegRepository.ObtieneConfigAPESEG();


        //        //int batchSize = configAPESEG.BatchSize;
        //        int delayInSeconds = configAPESEG.DelayInSeconds;

        //        //List<PlacaAPESEG> placasDistintas = Placas.GroupBy(p => p.Placa)
        //        //                         .Select(g => g.First())
        //        //                         .ToList();

        //        //Placas = placasDistintas;
        //        consultaMasivaRequest.CantidadTotal = Placas.Count;

        //        for (int i = 0; i < Placas.Count; i += 1)
        //        {
        //            var placaAPESEG = Placas[i];

        //            DataRow newRow = dtLista.NewRow();
        //            newRow["IdProceso"] = consultaMasivaRequest.idProceso;
        //            newRow["Placa"] = placaAPESEG.Placa;
        //            newRow["Orden"] = i + 1;
        //            dtLista.Rows.Add(newRow);
        //        }

        //        result = await apesegRepository.InsertaPlacasMasivo(consultaMasivaRequest, dtLista);
        //        Log.save(this, "InsertaPlacasMasivo");
        //        foreach (DataRow row in dtLista.Rows)
        //        {
        //            Consultar entitieApeseg = await apesegRepository.Consultar(row["Placa"].ToString());

        //            ApesegConsultaPlacaMasivoDetalle Placa = new ApesegConsultaPlacaMasivoDetalle();
        //            Placa.IdProceso = consultaMasivaRequest.idProceso;
        //            Placa.Placa = row["Placa"].ToString();
        //            Placa.Orden = row.Field<int>("Orden");
        //            Placa.Revisado = "Si";

        //            if (entitieApeseg.Certificados != null)
        //            {
        //                //Consulta certificado = entitieApeseg.Certificados.OrderByDescending(c => DateTime.Parse(c.FechaFin)).FirstOrDefault();
        //                Consulta certificado = entitieApeseg.Certificados.Select(c =>
        //                {
        //                    DateTime.TryParseExact(c.FechaFin, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fechaFin);
        //                    return new { Certificado = c, FechaFin = fechaFin };
        //                }).OrderByDescending(item => item.FechaFin).FirstOrDefault()?.Certificado;

        //                Placa.NombreCompania = certificado?.NombreCompania;
        //                Placa.FechaInicio = certificado?.FechaInicio;
        //                Placa.FechaFinVigencia = certificado?.FechaFinVigencia;
        //                Placa.FechaFin = certificado?.FechaFin;
        //                Placa.FechaInicioVigencia = certificado?.FechaInicioVigencia;
        //                Placa.FechaInicioD = certificado?.FechaInicioD;
        //                Placa.FechaFinD = certificado?.FechaFinD;
        //                Placa.Certificado = certificado?.Certificado;
        //                Placa.NombreUsoVehiculo = certificado?.NombreUsoVehiculo;
        //                Placa.NombreClaseVehiculo = certificado?.NombreClaseVehiculo;
        //                Placa.Estado = certificado?.Estado;
        //                Placa.CodigoSBSAseguradora = certificado?.CodigoSBSAseguradora;
        //                Placa.CodigoUnicoPoliza = certificado?.CodigoUnicoPoliza;
        //                Placa.EstaAnulado = certificado?.EstaAnulado;

        //            }

        //            result = await apesegRepository.ActualizaPlacasMasivo(consultaMasivaRequest, Placa);

        //            if (result.Mensaje == "CA")
        //            {
        //                break; // Termina el ciclo por cancelacion
        //            }

        //            //if (row.Field<int>("Orden") == 10)
        //            //{
        //            //    throw new Exception("Erro al consultar api apeseg PRUEBA!!");
        //            //}

        //            //Thread.Sleep(delayInSeconds * 1000);
        //            await Task.Delay(delayInSeconds * 1000);

        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //    return result;
        //}

        //public async Task<List<ApesegConsultaPlacaMasivo>> ConsultarProcesosMasivos()
        //{
        //    List<ApesegConsultaPlacaMasivo> result;
        //    try
        //    {
        //        result = await apesegRepository.ConsultarProcesosMasivos();

        //        //result = _mapper.Map<OrdenPago>(entitieApeseg);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return result;
        //}

        //public async Task<List<ApesegConsultaPlacaMasivoDetalle>> ConsultarProcesoDetalle(ConsultaMasivaRequest request)
        //{
        //    List<ApesegConsultaPlacaMasivoDetalle> result;
        //    try
        //    {
        //        result = await apesegRepository.ConsultarProcesoDetalle(request);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return result;
        //}

        //public async Task<ConsultaMasivaResponse> ActualizaErrorPlacasMasivo(ConsultaMasivaRequest consultaMasivaRequest)
        //{
        //    ConsultaMasivaResponse result;
        //    try
        //    {
        //        result = await apesegRepository.ActualizaErrorPlacasMasivo(consultaMasivaRequest);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return result;

        //}

        //public async Task<ConsultaMasivaResponse> ActualizaProcesoMasivo(ConsultaMasivaRequest request)
        //{
        //    ConsultaMasivaResponse result;
        //    try
        //    {
        //        result = await apesegRepository.ActualizaProcesoMasivo(request);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return result;
        //}

        //public async Task<EnvioCarritoResponse> InsertaEnvioCarrito(EnvioCarritoRequest request)
        //{
        //    var result = await apesegRepository.InsertaEnvioCarrito(request);

        //    return result;
        //}

        //public async Task<EnvioCarritoResponse> ActualizaEnvioCarrito(IEnumerable<string> placas, int toque)
        //{
        //    Log.save(this, $"EMPIEZA WEBAPI ActualizaEnvioCarrito Placa");
        //    var request = new ActualizaCarritoRequest();
        //    var subKey = await _helperRepository.GetValorTablaConfig("APESEG.SubscriptionKey");

        //    foreach (var placa in placas)
        //    {
        //        Log.save(this, $"EMPIEZA WEBAPI Consultar APESEG Placa:{placa} ");
        //        var datos = await Consultar(placa, subKey);
        //        var (esCertificadoValido, ultimoCertificado) = IsCertificadoValido(datos.Certificados);

        //        var placaRequest = new PlacaCarritoRequest
        //        {
        //            Placa = placa,
        //            Comprado = (datos.OperacionExitosa && datos.Certificados.Any()) ? esCertificadoValido : false,
        //            Intento = datos.OperacionExitosa ? 0 : 1,
        //            NombreCompania = ultimoCertificado?.NombreCompania,
        //            FechaInicioApeseg = ultimoCertificado?.FechaInicio,
        //            FechaFinApeseg = ultimoCertificado?.FechaFin
        //        };

        //        request.Placas.Add(placaRequest);

        //        Log.save(this, $"FIN WEBAPI Consultar APESEG Placa:{placa} - {JsonConvert.SerializeObject(placaRequest)}");
        //    }

        //    var result = await apesegRepository.ActualizaEnvioCarrito(request);
        //    var placasNoCompradas = request.Placas.Where(x => !x.Comprado).Select(x => x.Placa);
        //    var notificaciones = await NotificaEnvioCarrito(placasNoCompradas, toque);

        //    Log.save(this, $"FIN WEBAPI ActualizaEnvioCarrito");

        //    return result;
        //}

        //private (bool, Consulta?) IsCertificadoValido(IEnumerable<Consulta> certificados)
        //{
        //    if (certificados is null || !certificados.Any())
        //    {
        //        return (false, null);
        //    }

        //    var result = false;

        //    var ultimoCertificado = certificados
        //        .Where(x => DateTime.TryParse(x.FechaFin, out _))
        //        .OrderBy(x => DateTime.Parse(x.FechaFin))
        //        .LastOrDefault();

        //    if (ultimoCertificado != null && ultimoCertificado.Estado != VENCIDO)
        //    {
        //        result = true;
        //    }

        //    return (result, ultimoCertificado);
        //}

        //public async Task<NotificaCarritoResponse> NotificaEnvioCarrito(IEnumerable<string> placas, int toque)
        //{
        //    Log.save(this, $"EMPIEZA WEBAPI NotificaEnvioCarrito");

        //    var result = new NotificaCarritoResponse();
        //    var AdjuntosCorreo = await _helperRepository.GetValorTablaConfig("ENVIOCARRITO.ENVIOCORREO.URL");
        //    var AdjuntosWhatsApp = await _helperRepository.GetValorTablaConfig("ENVIOCARRITO.ENVIOWHATSAPP.URL");

        //    var urlsAdjuntoCorreo = JsonConvert.DeserializeObject<EnvioCarritoAdjunto>(AdjuntosCorreo);
        //    var urlsAdjuntoWhatsApp = JsonConvert.DeserializeObject<EnvioCarritoAdjunto>(AdjuntosWhatsApp);

        //    var urlAdjuntoCorreo = toque switch
        //    {
        //        1 => urlsAdjuntoCorreo.Toque1,
        //        2 => urlsAdjuntoCorreo.Toque2,
        //        3 => urlsAdjuntoCorreo.Toque3,
        //        _ => throw new ArgumentException("Valor de toque no válido.")
        //    };

        //    var urlAdjuntoWhatsApp = toque switch
        //    {
        //        1 => urlsAdjuntoWhatsApp.Toque1,
        //        2 => urlsAdjuntoWhatsApp.Toque2,
        //        3 => urlsAdjuntoWhatsApp.Toque3,
        //        _ => throw new ArgumentException("Valor de toque no válido.")
        //    };

        //    foreach (var placa in placas)
        //    {
        //        Log.save(this, $"EMPIEZA WEBAPI Notificar Placa:{placa} ");
        //        var notificacion = new NotificacionPlaca { Placa = placa };
        //        var datosCliente = await _autoService.ConsultarTrackingInfo(notificacion.Placa);

        //        datosCliente.Adjunto = urlAdjuntoCorreo;
        //        var resultCorreo = await NotificaCorreoEnvioCarrito(datosCliente, toque);

        //        notificacion.CorreoEnviado = resultCorreo == Constants.Status.OK;

        //        datosCliente.Adjunto = urlAdjuntoWhatsApp;
        //        var resultWhatsApp = await NotificaWhatsAppEnvioCarrito(datosCliente, toque);

        //        notificacion.WhatsAppEnviado = resultWhatsApp == Constants.Status.OK;

        //        result.Placas.Add(notificacion);

        //        Log.save(this, $"FIN WEBAPI Notificar Placa:{placa} - {JsonConvert.SerializeObject(notificacion)}");
        //    }

        //    Log.save(this, $"FIN WEBAPI NotificaEnvioCarrito");

        //    return result;
        //}

        //public async Task<string> NotificaCorreoEnvioCarrito(TrackingInfo cliente, int toque)
        //{
        //    Log.save(this, "EMPIEZA NotificaCorreoEnvioCarrito");

        //    var result = string.Empty;
        //    var clienteNombres = $"{cliente.Nombre1} {cliente.ApePaterno}";
        //    var mailParam = new CorreoParams()
        //    {
        //        Para_Correo = cliente.Correo,
        //        Para_Nombre = clienteNombres,
        //        Asunto = new string[] { "Compra Carrito - SOAT", clienteNombres },
        //        Mensaje = new string[]
        //        { cliente.Placa,
        //          clienteNombres,
        //          cliente.Prima.ToString()
        //        }
        //    };

        //    // Asignar tipoCorreo según el valor de toque
        //    var tipoCorreo = toque switch
        //    {
        //        1 => _appSettings.TipoCorreoT1,
        //        2 => _appSettings.TipoCorreoT2,
        //        3 => _appSettings.TipoCorreoT3,
        //        _ => throw new ArgumentException("Valor de toque no válido.")
        //    };

        //    var mailresponse = await _correoCentral.Enviar(mailParam, tipoCorreo);

        //    if (mailresponse != 1)
        //    {
        //        //throw new ArgumentException("Error de envio correo Anulación.");
        //        return Constants.Status.ERROR;
        //    }

        //    result = Constants.Status.OK;

        //    Log.save(this, "TERMINA NotificaCorreoEnvioCarrito");
        //    return result;
        //}

        //public async Task<string> NotificaWhatsAppEnvioCarrito(TrackingInfo cliente, int toque)
        //{
        //    Log.save(this, "EMPIEZA NotificaWhatsAppEnvioCarrito");

        //    var result = string.Empty;
        //    var clienteNombres = $"{cliente.Nombre1} {cliente.ApePaterno}";
        //    string fileName = Path.GetFileName(new Uri(cliente.Adjunto).LocalPath);
        //    var whatsAppParam = new WhatsAppParam()
        //    {
        //        Nombre = clienteNombres,
        //        Celular = cliente.Celular,
        //        Adjunto = cliente.Adjunto,
        //        Parametros = new string[] { clienteNombres, cliente.Prima.ToString(), fileName }
        //    };

        //    // Asignar tipoCorreo según el valor de toque
        //    var tipoWhatsApp = toque switch
        //    {
        //        1 => _appSettings.TipoWhatsappT1,
        //        2 => _appSettings.TipoWhatsappT2,
        //        3 => _appSettings.TipoWhatsappT3,
        //        _ => throw new ArgumentException("Valor de toque no válido.")
        //    };

        //    Log.save(this, $"EMPIEZA WhatsAppCentral.Enviar  - {JsonConvert.SerializeObject(whatsAppParam)} -  Tipo: {tipoWhatsApp}");

        //    var mailresponse = await _whatsAppCentral.Enviar(whatsAppParam, tipoWhatsApp);

        //    Log.save(this, "EMPIEZA WhatsAppCentral.Enviar");

        //    if (mailresponse != 1)
        //    {
        //        //throw new ArgumentException("Error de envio correo Anulación.");
        //        return Constants.Status.ERROR;
        //    }

        //    result = Constants.Status.OK;

        //    Log.save(this, "TERMINA NotificaWhatsAppEnvioCarrito");
        //    return result;
        //}


    }
}