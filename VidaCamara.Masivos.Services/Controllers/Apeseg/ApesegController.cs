using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using VidaCamara.CrossCuting.Log.Contracts;
using VidaCamara.CrossCuting.Utilities;
using VidaCamara.CrossCuting.Utilities.Configuration;
using VidaCamara.Domain.Services.Entidades.Apeseg;
using VidaCamara.Masivos.Services.Services.Apeseg;

namespace VidaCamara.Masivos.Services.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class ApesegController : Controller
    {
        #region Atributos Privados
        private readonly IApesegService _apesegService;
        private readonly AppSettings _appSettings;
        #endregion

        #region Constructor
        public ApesegController(IApesegService apesegService, IOptions<AppSettings> appSettings)
        {
            _apesegService = apesegService;
            _appSettings = appSettings.Value;
        }
        #endregion

        #region Métodos Públicos (Endpoints)

        [HttpPost]
        [Route("Apeseg_Registrar")]
        public async Task<IActionResult> Apeseg_Registrar([FromBody] RegistroSOATRequest _param)
        {
            if (!ModelState.IsValid)
            {
                var modelErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { success = false, message = "ERROR: Mala petición (Modelo inválido)", data = (object)null, errors = modelErrors });
            }

            ApesegLog apesegLog = null;
            object respuestaFinal = null;

            try
            {
                Log.saveFirstLine();
                Log.save(this, "EMPIEZA GRABACIÓN A APESEG REGISTRAR PLACA= " + _param.PlacaVehiculo);
                _param.IpCliente = GetClientIp();

                #region 1. Validación de Reglas de Negocio (Base de Datos / Service)
                Log.save(this, "EMPIEZA VALIDACION API REGISTRAR PLACA= " + _param.PlacaVehiculo);
                var errores = await _apesegService.Apeseg_Validar("R", _param);
                if (errores.Any())
                {
                    return BadRequest(new { success = false, message = $"ERROR API: Error en validación para grabación placa {_param.PlacaVehiculo}", data = (object)null, errors = errores });
                }
                Log.save(this, "TERMINA VALIDACION API REGISTRAR PLACA= " + _param.PlacaVehiculo);
                #endregion

                #region 2. Ejecución de la Operación Principal (Consumo APESEG)
                var (datos, logRetornado) = await _apesegService.Apeseg_Registrar(_param);
                apesegLog = logRetornado;

                if (!datos.OperacionExitosa)
                {
                    respuestaFinal = new { success = false, message = $"ERROR APESEG: Error en grabación placa {_param.PlacaVehiculo}", data = (object)null, errors = datos.MensajeError ?? new List<string> { $"{datos.CodigoError}" } };

                    #region 3. Log de servicio y Persistencia (Fallo de Negocio)
                    if (apesegLog != null)
                    {
                        apesegLog.TramaEnvio = Newtonsoft.Json.JsonConvert.SerializeObject(_param);
                        apesegLog.TramaRespuesta = Newtonsoft.Json.JsonConvert.SerializeObject(respuestaFinal);
                        await _apesegService.Apeseg_Insertar(apesegLog);
                    }
                    #endregion

                    return BadRequest(respuestaFinal);
                }
                #endregion

                #region 3. Log de servicio y Persistencia (Flujo Exitoso)
                Log.save(this, "EMPIEZA PROCESO DE GRABARLOG APESEG PLACA= " + _param.PlacaVehiculo);

                respuestaFinal = new { success = true, message = "OK", data = datos, errors = (object)null };

                if (apesegLog != null)
                {
                    apesegLog.TramaEnvio = Newtonsoft.Json.JsonConvert.SerializeObject(_param);
                    apesegLog.TramaRespuesta = Newtonsoft.Json.JsonConvert.SerializeObject(respuestaFinal);
                    await _apesegService.Apeseg_Insertar(apesegLog);
                }

                Log.save(this, "TERMINA PROCESO DE GRABARLOG APESEG PLACA= " + _param.PlacaVehiculo);
                #endregion

                Log.save(this, "TERMINA GRABACIÓN A APESEG REGISTRAR PLACA= " + _param.PlacaVehiculo);
                return Ok(respuestaFinal);
            }
            catch (Exception ex)
            {
                #region 4. Control y Persistencia de Excepciones Críticas
                Log.save(this, "ERROR: " + ex.Message);

                respuestaFinal = new { success = false, message = $"ERROR API: Error en grabación placa {_param.PlacaVehiculo}", data = (object)null, errors = new List<string> { ex.Message } };

                if (apesegLog == null)
                {
                    apesegLog = new ApesegLog
                    {
                        Tipo = "R",
                        Nro = _param.PolizaCertificado,
                        Digito = -1,
                        Err = "Error Crítico Controller: " + ex.Message,
                        Envia = "",
                        Recibe = "",
                        User = _param.UsuarioRegistro,
                        Proveedor = _param.Proveedor,
                        Canal = _param.Canal,
                        PuntoVenta = _param.PuntoVenta,
                        IpCliente = _param.IpCliente
                    };
                }
                else
                {
                    apesegLog.Err = "Error post-invocación: " + ex.Message;
                }

                apesegLog.TramaEnvio = Newtonsoft.Json.JsonConvert.SerializeObject(_param);
                apesegLog.TramaRespuesta = Newtonsoft.Json.JsonConvert.SerializeObject(respuestaFinal);

                try
                {
                    await _apesegService.Apeseg_Insertar(apesegLog);
                }
                catch (Exception exLog)
                {
                    Log.save(this, "CRÍTICO: Falló la inserción del log: " + exLog.Message);
                }

                return BadRequest(respuestaFinal);
                #endregion
            }
        }

        [HttpPut]
        [Route("Apeseg_Actualizar")]
        public async Task<IActionResult> Apeseg_Actualizar([FromBody] ModificarSOATRequest _param)
        {
            if (!ModelState.IsValid)
            {
                var modelErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { success = false, message = "ERROR: Mala petición (Modelo inválido)", data = (object)null, errors = modelErrors });
            }

            ApesegLog apesegLog = null;
            object respuestaFinal = null;

            try
            {
                Log.saveFirstLine();
                Log.save(this, "EMPIEZA MODIFICACIÓN A APESEG PLACA= " + _param.PlacaVehiculo);
                _param.IpCliente = GetClientIp();

                #region 1. Validación de Reglas de Negocio (Base de Datos / Service)
                Log.save(this, "EMPIEZA VALIDACION API ACTUALIZAR PLACA= " + _param.PlacaVehiculo);
                var errores = await _apesegService.Apeseg_Validar("U", _param);
                if (errores.Any())
                {
                    return BadRequest(new { success = false, message = $"ERROR API: Error en validación para actualización placa {_param.PlacaVehiculo}", data = (object)null, errors = errores });
                }
                Log.save(this, "TERMINA VALIDACION API ACTUALIZAR PLACA= " + _param.PlacaVehiculo);
                #endregion

                #region 2. Ejecución de la Operación Principal (Consumo APESEG)
                var (datos, logRetornado) = await _apesegService.Apeseg_Actualizar(_param);
                apesegLog = logRetornado;

                if (!datos.OperacionExitosa)
                {
                    respuestaFinal = new { success = false, message = $"ERROR APESEG: Error en actualización placa {_param.PlacaVehiculo}", data = (object)null, errors = datos.MensajeError ?? new List<string> { $"{datos.CodigoError}" } };

                    #region 3. Log de servicio y Persistencia (Fallo de Negocio)
                    if (apesegLog != null)
                    {
                        apesegLog.TramaEnvio = Newtonsoft.Json.JsonConvert.SerializeObject(_param);
                        apesegLog.TramaRespuesta = Newtonsoft.Json.JsonConvert.SerializeObject(respuestaFinal);
                        await _apesegService.Apeseg_Insertar(apesegLog);
                    }
                    #endregion

                    return BadRequest(respuestaFinal);
                }
                #endregion

                #region 3. Log de servicio y Persistencia (Flujo Exitoso)
                Log.save(this, "EMPIEZA PROCESO DE GRABARLOG APESEG PLACA= " + _param.PlacaVehiculo);

                respuestaFinal = new { success = true, message = "OK", data = datos, errors = (object)null };

                if (apesegLog != null)
                {
                    apesegLog.TramaEnvio = Newtonsoft.Json.JsonConvert.SerializeObject(_param);
                    apesegLog.TramaRespuesta = Newtonsoft.Json.JsonConvert.SerializeObject(respuestaFinal);
                    await _apesegService.Apeseg_Insertar(apesegLog);
                }

                Log.save(this, "TERMINA PROCESO DE GRABARLOG APESEG PLACA= " + _param.PlacaVehiculo);
                #endregion

                Log.save(this, "TERMINA MODIFICACIÓN A APESEG PLACA= " + _param.PlacaVehiculo);
                return Ok(respuestaFinal);
            }
            catch (Exception ex)
            {
                #region 4. Control y Persistencia de Excepciones Críticas
                Log.save(this, "ERROR: " + ex.Message);

                respuestaFinal = new { success = false, message = $"ERROR API: Error en actualización placa {_param.PlacaVehiculo}", data = (object)null, errors = new List<string> { ex.Message } };

                if (apesegLog == null)
                {
                    apesegLog = new ApesegLog
                    {
                        Tipo = "U",
                        Nro = _param.PolizaCertificado,
                        Digito = -1,
                        Err = "Error Crítico Controller: " + ex.Message,
                        Envia = "",
                        Recibe = "",
                        User = _param.UsuarioRegistro,
                        Proveedor = _param.Proveedor,
                        Canal = _param.Canal,
                        PuntoVenta = _param.PuntoVenta,
                        IpCliente = _param.IpCliente
                    };
                }
                else
                {
                    apesegLog.Err = "Error post-invocación: " + ex.Message;
                }

                apesegLog.TramaEnvio = Newtonsoft.Json.JsonConvert.SerializeObject(_param);
                apesegLog.TramaRespuesta = Newtonsoft.Json.JsonConvert.SerializeObject(respuestaFinal);

                try
                {
                    await _apesegService.Apeseg_Insertar(apesegLog);
                }
                catch (Exception exLog)
                {
                    Log.save(this, "CRÍTICO: Falló la inserción del log: " + exLog.Message);
                }

                return BadRequest(respuestaFinal);
                #endregion
            }
        }

        [HttpPut]
        [Route("Apeseg_Anular")]
        public async Task<IActionResult> Apeseg_Anular([FromBody] AnulacionSOATRequest _param)
        {
            if (!ModelState.IsValid)
            {
                var modelErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { success = false, message = "ERROR: Mala petición (Modelo inválido)", data = (object)null, errors = modelErrors });
            }

            ApesegLog apesegLog = null;
            object respuestaFinal = null;

            try
            {
                Log.saveFirstLine();
                Log.save(this, "EMPIEZA ANULACIÓN A APESEG CERTIFICADO= " + _param.PolizaCertificado);
                _param.IpCliente = GetClientIp();

                #region 1. Validación de Reglas de Negocio (Base de Datos / Service)
                Log.save(this, "EMPIEZA VALIDACION API ANULAR CERTIFICADO= " + _param.PolizaCertificado);
                var errores = await _apesegService.Apeseg_Validar("D", _param);
                if (errores.Any())
                {
                    return BadRequest(new { success = false, message = $"ERROR API: Error en validación para anulación certificado {_param.PolizaCertificado}", data = (object)null, errors = errores });
                }
                Log.save(this, "TERMINA VALIDACION API ANULAR CERTIFICADO= " + _param.PolizaCertificado);
                #endregion

                #region 2. Ejecución de la Operación Principal (Consumo APESEG)
                var (datos, logRetornado) = await _apesegService.Apeseg_Anular(_param);
                apesegLog = logRetornado;

                if (!datos.OperacionExitosa)
                {
                    respuestaFinal = new { success = false, message = $"ERROR APESEG: Error en anulación certificado {_param.PolizaCertificado}", data = (object)null, errors = datos.MensajeError ?? new List<string> { $"{datos.CodigoError}" } };

                    #region 3. Log de servicio y Persistencia (Fallo de Negocio)
                    if (apesegLog != null)
                    {
                        apesegLog.TramaEnvio = Newtonsoft.Json.JsonConvert.SerializeObject(_param);
                        apesegLog.TramaRespuesta = Newtonsoft.Json.JsonConvert.SerializeObject(respuestaFinal);
                        await _apesegService.Apeseg_Insertar(apesegLog);
                    }
                    #endregion

                    return BadRequest(respuestaFinal);
                }
                #endregion

                #region 3. Log de servicio y Persistencia (Flujo Exitoso)
                Log.save(this, "EMPIEZA PROCESO DE GRABARLOG APESEG CERTIFICADO= " + _param.PolizaCertificado);

                respuestaFinal = new { success = true, message = "OK", data = datos, errors = (object)null };

                if (apesegLog != null)
                {
                    apesegLog.TramaEnvio = Newtonsoft.Json.JsonConvert.SerializeObject(_param);
                    apesegLog.TramaRespuesta = Newtonsoft.Json.JsonConvert.SerializeObject(respuestaFinal);
                    await _apesegService.Apeseg_Insertar(apesegLog);
                }

                Log.save(this, "TERMINA PROCESO DE GRABARLOG APESEG CERTIFICADO= " + _param.PolizaCertificado);
                #endregion

                Log.save(this, "TERMINA ANULACIÓN A APESEG CERTIFICADO= " + _param.PolizaCertificado);
                return Ok(respuestaFinal);
            }
            catch (Exception ex)
            {
                #region 4. Control y Persistencia de Excepciones Críticas
                Log.save(this, "ERROR: " + ex.Message);

                respuestaFinal = new { success = false, message = $"ERROR API: Error en anulación certificado {_param.PolizaCertificado}", data = (object)null, errors = new List<string> { ex.Message } };

                if (apesegLog == null)
                {
                    apesegLog = new ApesegLog
                    {
                        Tipo = "D",
                        Nro = _param.PolizaCertificado,
                        Digito = -1,
                        Err = "Error Crítico Controller: " + ex.Message,
                        Envia = "",
                        Recibe = "",
                        User = _param.UsuarioRegistro,
                        Proveedor = _param.Proveedor,
                        Canal = _param.Canal,
                        PuntoVenta = _param.PuntoVenta,
                        IpCliente = _param.IpCliente
                    };
                }
                else
                {
                    apesegLog.Err = "Error post-invocación: " + ex.Message;
                }

                apesegLog.TramaEnvio = Newtonsoft.Json.JsonConvert.SerializeObject(_param);
                apesegLog.TramaRespuesta = Newtonsoft.Json.JsonConvert.SerializeObject(respuestaFinal);

                try
                {
                    await _apesegService.Apeseg_Insertar(apesegLog);
                }
                catch (Exception exLog)
                {
                    Log.save(this, "CRÍTICO: Falló la inserción del log: " + exLog.Message);
                }

                return BadRequest(respuestaFinal);
                #endregion
            }
        }

        [HttpPost]
        [Route("Consultar")]
        public async Task<IActionResult> Consultar([FromBody] ConsultaSOATRequest _param)
        {
            if (!ModelState.IsValid)
            {
                var modelErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { success = false, message = "ERROR: Mala petición (Modelo inválido)", data = (object)null, errors = modelErrors });
            }

            ApesegLog apesegLog = null;
            object respuestaFinal = null;

            try
            {
                Log.saveFirstLine();
                Log.save(this, "EMPIEZA Consultar A APESEG / _placa= " + _param.placa);
                _param.IpCliente = GetClientIp();

                #region 1. Validación de Reglas de Negocio (Base de Datos / Service)
                Log.save(this, "EMPIEZA VALIDACION API CONSULTA PLACA= " + _param.placa);
                var errores = await _apesegService.Apeseg_Validar("C", _param);
                if (errores.Any())
                {
                    return BadRequest(new { success = false, message = $"ERROR API: Error en consulta de placa {_param.placa}", data = (object)null, errors = errores });
                }
                Log.save(this, "TERMINA VALIDACION API CONSULTA PLACA= " + _param.placa);
                #endregion

                #region 2. Ejecución de la Operación Principal (Consumo APESEG)
                var (datos, logRetornado) = await _apesegService.Consultar(_param);
                apesegLog = logRetornado;

                Log.save(this, "TERMINA Consultar A APESEG / _placa= " + _param.placa);

                if (datos is null)
                {
                    respuestaFinal = new { success = false, message = "ERROR: No se obtuvo respuesta de APESEG", data = (object)null, errors = new List<string> { "La respuesta de consulta externa retornó un valor nulo." } };

                    #region 3. Log de servicio y Persistencia (Fallo de Negocio)
                    if (apesegLog != null)
                    {
                        apesegLog.TramaEnvio = Newtonsoft.Json.JsonConvert.SerializeObject(_param);
                        apesegLog.TramaRespuesta = Newtonsoft.Json.JsonConvert.SerializeObject(respuestaFinal);
                        await _apesegService.Apeseg_Insertar(apesegLog);
                    }
                    #endregion

                    return BadRequest(respuestaFinal);
                }
                #endregion

                #region 3. Formatear salida de certificado
                if (datos.Certificados != null && datos.Certificados.Any())
                {
                    var filter = datos.Certificados.Count == 1
                                 ? datos.Certificados.First()
                                 : datos.Certificados.Where(x => x.FechaFin != null)
                                                     .OrderByDescending(x => DateTime.Parse(x.FechaFin))
                                                     .FirstOrDefault();

                    if (filter != null)
                    {
                        filter.FechaInicioD = string.IsNullOrEmpty(filter.FechaInicio) ? null : Convert.ToDateTime(filter.FechaInicio);
                        filter.FechaFinD = string.IsNullOrEmpty(filter.FechaFin) ? null : Convert.ToDateTime(filter.FechaFin);

                        datos.Certificados.Clear();
                        datos.Certificados.Add(filter);
                    }
                }
                #endregion

                #region 3. Log de servicio y Persistencia (Flujo Exitoso)
                Log.save(this, "EMPIEZA PROCESO DE GRABARLOG APESEG PLACA= " + _param.placa);

                respuestaFinal = new { success = true, message = "OK", data = datos, errors = (object)null };

                if (apesegLog != null)
                {
                    apesegLog.TramaEnvio = Newtonsoft.Json.JsonConvert.SerializeObject(_param);
                    apesegLog.TramaRespuesta = Newtonsoft.Json.JsonConvert.SerializeObject(respuestaFinal);
                    await _apesegService.Apeseg_Insertar(apesegLog);
                }

                Log.save(this, "TERMINA PROCESO DE GRABARLOG APESEG PLACA= " + _param.placa);
                #endregion

                return Ok(respuestaFinal);
            }
            catch (Exception ex)
            {
                #region 4. Control y Persistencia de Excepciones Críticas
                Log.save(this, "ERROR: " + ex.Message);

                respuestaFinal = new { success = false, message = $"ERROR API: Error al consultar la placa {_param.placa}", data = (object)null, errors = new List<string> { ex.Message } };

                if (apesegLog == null)
                {
                    apesegLog = new ApesegLog
                    {
                        Tipo = "C",
                        Nro = null,
                        Digito = -1,
                        Err = "Error Crítico Controller: " + ex.Message,
                        Envia = "",
                        Recibe = "",
                        User = _param.UsuarioRegistro,
                        Proveedor = _param.Proveedor,
                        Canal = _param.Canal,
                        PuntoVenta = _param.PuntoVenta,
                        IpCliente = _param.IpCliente
                    };
                }
                else
                {
                    apesegLog.Err = "Error post-invocación: " + ex.Message;
                }

                apesegLog.TramaEnvio = Newtonsoft.Json.JsonConvert.SerializeObject(_param);
                apesegLog.TramaRespuesta = Newtonsoft.Json.JsonConvert.SerializeObject(respuestaFinal);

                try
                {
                    await _apesegService.Apeseg_Insertar(apesegLog);
                }
                catch (Exception exLog)
                {
                    Log.save(this, "CRÍTICO: Falló la inserción del log: " + exLog.Message);
                }

                return BadRequest(respuestaFinal);
                #endregion
            }
        }

        #endregion

        #region Métodos Privados
        private string GetClientIp()
        {
            string ip = Request.Headers["X-Forwarded-For"].FirstOrDefault();

            if (string.IsNullOrEmpty(ip))
            {
                ip = HttpContext.Connection.RemoteIpAddress?.ToString();
                if (ip == "::1")
                    return "127.0.0.1";
            }
            return ip ?? "127.0.0.1";
        }
        #endregion
    }
}