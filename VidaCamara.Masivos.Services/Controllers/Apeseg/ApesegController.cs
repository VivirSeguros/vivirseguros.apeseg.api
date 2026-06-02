using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
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
            string ipCliente = GetClientIp();
            ApesegLog apesegLog = null;
            object respuestaFinal = null;

            try
            {
                // 1. Validación de Request (ModelState)
                if (!ModelState.IsValid)
                {
                    var modelErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    respuestaFinal = new { success = false, message = "ERROR: Mala petición (Modelo inválido)", data = (object)null, errors = modelErrors };

                    throw new Exception("Fallo de validación en ModelState: " + string.Join(" | ", modelErrors));
                }

                _param.IpCliente = ipCliente;

                // Inicialización del Log para flujo exitoso o fallos posteriores
                apesegLog = new ApesegLog
                {
                    Tipo = "R",
                    Nro = _param.PolizaCertificado,
                    Digito = -1,
                    User = _param.UsuarioRegistro,
                    Proveedor = _param.Proveedor,
                    Canal = _param.Canal,
                    PuntoVenta = _param.PuntoVenta,
                    IpCliente = _param.IpCliente,
                    TramaEnvio = Newtonsoft.Json.JsonConvert.SerializeObject(_param),
                    Envia = "",
                    Recibe = ""
                };

                Log.saveFirstLine();
                Log.save(this, "EMPIEZA GRABACIÓN A APESEG REGISTRAR PLACA= " + _param.PlacaVehiculo);

                // 2. Validación de Reglas de Negocio Locales
                Log.save(this, "EMPIEZA VALIDACION API REGISTRAR PLACA= " + _param.PlacaVehiculo);
                var errores = await _apesegService.Apeseg_Validar("R", _param);
                if (errores.Any())
                {
                    respuestaFinal = new { success = false, message = $"ERROR API: Error en validación para grabación placa {_param.PlacaVehiculo}", data = (object)null, errors = errores };
                    throw new Exception("Fallo en validación de reglas de negocio locales (Validar).");
                }
                Log.save(this, "TERMINA VALIDACION API REGISTRAR PLACA= " + _param.PlacaVehiculo);

                // 3. Ejecución de la Operación Principal (Consumo APESEG)
                var (datos, logRetornado) = await _apesegService.Apeseg_Registrar(_param);

                if (logRetornado != null)
                {
                    apesegLog = logRetornado;
                    apesegLog.TramaEnvio = Newtonsoft.Json.JsonConvert.SerializeObject(_param);
                }

                if (!datos.OperacionExitosa)
                {
                    var errorsApeseg = datos.MensajeError ?? new List<string> { $"{datos.CodigoError}" };
                    respuestaFinal = new { success = false, message = $"ERROR APESEG: Error en grabación placa {_param.PlacaVehiculo}", data = (object)null, errors = errorsApeseg };
                    throw new Exception("Error devuelto por la plataforma externa APESEG.");
                }

                // 4. Flujo de Respuesta Exitosa
                Log.save(this, "EMPIEZA PROCESO DE GRABARLOG APESEG PLACA= " + _param.PlacaVehiculo);

                respuestaFinal = new { success = true, message = "OK", data = datos, errors = (object)null };
                apesegLog.TramaRespuesta = Newtonsoft.Json.JsonConvert.SerializeObject(respuestaFinal);

                await Apeseg_Insertar(apesegLog);

                Log.save(this, "TERMINA PROCESO DE GRABARLOG APESEG PLACA= " + _param.PlacaVehiculo);
                Log.save(this, "TERMINA GRABACIÓN A APESEG REGISTRAR PLACA= " + _param.PlacaVehiculo);

                return Ok(respuestaFinal);
            }
            catch (Exception ex)
            {
                Log.save(this, "ERROR: " + ex.Message);

                if (apesegLog == null)
                {
                    apesegLog = new ApesegLog
                    {
                        Tipo = "R",
                        Nro = _param?.PolizaCertificado,
                        Digito = -1,
                        User = _param?.UsuarioRegistro ?? "Desconocido",
                        Proveedor = _param?.Proveedor,
                        Canal = _param?.Canal,
                        PuntoVenta = _param?.PuntoVenta,
                        IpCliente = ipCliente,
                        TramaEnvio = _param != null ? Newtonsoft.Json.JsonConvert.SerializeObject(_param) : "Request Body Nulo",
                        Envia = "",
                        Recibe = ""
                    };
                }

                if (respuestaFinal == null)
                {
                    respuestaFinal = new { success = false, message = $"ERROR API: Error en grabación placa {_param?.PlacaVehiculo}", data = (object)null, errors = new List<string> { ex.Message } };
                }

                apesegLog.Err = string.IsNullOrEmpty(apesegLog.Err) ? ex.Message : apesegLog.Err;
                apesegLog.TramaRespuesta = Newtonsoft.Json.JsonConvert.SerializeObject(respuestaFinal);

                await Apeseg_Insertar(apesegLog);

                return BadRequest(respuestaFinal);
            }
        }

        [HttpPut]
        [Route("Apeseg_Actualizar")]
        public async Task<IActionResult> Apeseg_Actualizar([FromBody] ModificarSOATRequest _param)
        {
            string ipCliente = GetClientIp();
            ApesegLog apesegLog = null;
            object respuestaFinal = null;

            try
            {
                // 1. Validación de Request (ModelState)
                if (!ModelState.IsValid)
                {
                    var modelErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    respuestaFinal = new { success = false, message = "ERROR: Mala petición (Modelo inválido)", data = (object)null, errors = modelErrors };

                    throw new Exception("Fallo de validación en ModelState: " + string.Join(" | ", modelErrors));
                }

                _param.IpCliente = ipCliente;

                apesegLog = new ApesegLog
                {
                    Tipo = "U",
                    Nro = _param.PolizaCertificado,
                    Digito = -1,
                    User = _param.UsuarioRegistro,
                    Proveedor = _param.Proveedor,
                    Canal = _param.Canal,
                    PuntoVenta = _param.PuntoVenta,
                    IpCliente = _param.IpCliente,
                    TramaEnvio = Newtonsoft.Json.JsonConvert.SerializeObject(_param),
                    Envia = "",
                    Recibe = ""
                };

                Log.saveFirstLine();
                Log.save(this, "EMPIEZA MODIFICACIÓN A APESEG PLACA= " + _param.PlacaVehiculo);

                // 2. Validación de Reglas de Negocio Locales
                Log.save(this, "EMPIEZA VALIDACION API ACTUALIZAR PLACA= " + _param.PlacaVehiculo);
                var errores = await _apesegService.Apeseg_Validar("U", _param);
                if (errores.Any())
                {
                    respuestaFinal = new { success = false, message = $"ERROR API: Error en validación para actualización placa {_param.PlacaVehiculo}", data = (object)null, errors = errores };
                    throw new Exception("Fallo en validación de reglas de negocio locales (Actualizar).");
                }
                Log.save(this, "TERMINA VALIDACION API ACTUALIZAR PLACA= " + _param.PlacaVehiculo);

                // 3. Ejecución de la Operación Principal (Consumo APESEG)
                var (datos, logRetornado) = await _apesegService.Apeseg_Actualizar(_param);

                if (logRetornado != null)
                {
                    apesegLog = logRetornado;
                    apesegLog.TramaEnvio = Newtonsoft.Json.JsonConvert.SerializeObject(_param);
                }

                if (!datos.OperacionExitosa)
                {
                    var errorsApeseg = datos.MensajeError ?? new List<string> { $"{datos.CodigoError}" };
                    respuestaFinal = new { success = false, message = $"ERROR APESEG: Error en actualización placa {_param.PlacaVehiculo}", data = (object)null, errors = errorsApeseg };
                    throw new Exception("Error devuelto por la plataforma externa APESEG.");
                }

                // 4. Flujo de Respuesta Exitosa
                Log.save(this, "EMPIEZA PROCESO DE GRABARLOG APESEG PLACA= " + _param.PlacaVehiculo);

                respuestaFinal = new { success = true, message = "OK", data = datos, errors = (object)null };
                apesegLog.TramaRespuesta = Newtonsoft.Json.JsonConvert.SerializeObject(respuestaFinal);

                await Apeseg_Insertar(apesegLog);

                Log.save(this, "TERMINA PROCESO DE GRABARLOG APESEG PLACA= " + _param.PlacaVehiculo);
                Log.save(this, "TERMINA MODIFICACIÓN A APESEG PLACA= " + _param.PlacaVehiculo);

                return Ok(respuestaFinal);
            }
            catch (Exception ex)
            {
                Log.save(this, "ERROR: " + ex.Message);

                if (apesegLog == null)
                {
                    apesegLog = new ApesegLog
                    {
                        Tipo = "U",
                        Nro = _param?.PolizaCertificado,
                        Digito = -1,
                        User = _param?.UsuarioRegistro ?? "Desconocido",
                        Proveedor = _param?.Proveedor,
                        Canal = _param?.Canal,
                        PuntoVenta = _param?.PuntoVenta,
                        IpCliente = ipCliente,
                        TramaEnvio = _param != null ? Newtonsoft.Json.JsonConvert.SerializeObject(_param) : "Request Body Nulo",
                        Envia = "",
                        Recibe = ""
                    };
                }

                if (respuestaFinal == null)
                {
                    respuestaFinal = new { success = false, message = $"ERROR API: Error en actualización placa {_param?.PlacaVehiculo}", data = (object)null, errors = new List<string> { ex.Message } };
                }

                apesegLog.Err = string.IsNullOrEmpty(apesegLog.Err) ? ex.Message : apesegLog.Err ;
                apesegLog.TramaRespuesta = Newtonsoft.Json.JsonConvert.SerializeObject(respuestaFinal);

                await Apeseg_Insertar(apesegLog);

                return BadRequest(respuestaFinal);
            }
        }

        [HttpPut]
        [Route("Apeseg_Anular")]
        public async Task<IActionResult> Apeseg_Anular([FromBody] AnulacionSOATRequest _param)
        {
            string ipCliente = GetClientIp();
            ApesegLog apesegLog = null;
            object respuestaFinal = null;

            try
            {
                // 1. Validación de Request (ModelState)
                if (!ModelState.IsValid)
                {
                    var modelErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    respuestaFinal = new { success = false, message = "ERROR: Mala petición (Modelo inválido)", data = (object)null, errors = modelErrors };

                    throw new Exception("Fallo de validación en ModelState: " + string.Join(" | ", modelErrors));
                }

                _param.IpCliente = ipCliente;

                apesegLog = new ApesegLog
                {
                    Tipo = "D",
                    Nro = _param.PolizaCertificado,
                    Digito = -1,
                    User = _param.UsuarioRegistro,
                    Proveedor = _param.Proveedor,
                    Canal = _param.Canal,
                    PuntoVenta = _param.PuntoVenta,
                    IpCliente = _param.IpCliente,
                    TramaEnvio = Newtonsoft.Json.JsonConvert.SerializeObject(_param),
                    Envia = "",
                    Recibe = ""
                };

                Log.saveFirstLine();
                Log.save(this, "EMPIEZA ANULACIÓN A APESEG CERTIFICADO= " + _param.PolizaCertificado);

                // 2. Validación de Reglas de Negocio Locales
                Log.save(this, "EMPIEZA VALIDACION API ANULAR CERTIFICADO= " + _param.PolizaCertificado);
                var errores = await _apesegService.Apeseg_Validar("D", _param);
                if (errores.Any())
                {
                    respuestaFinal = new { success = false, message = $"ERROR API: Error en validación para anulación certificado {_param.PolizaCertificado}", data = (object)null, errors = errores };
                    throw new Exception("Fallo en validación de reglas de negocio locales (Anular).");
                }
                Log.save(this, "TERMINA VALIDACION API ANULAR CERTIFICADO= " + _param.PolizaCertificado);

                // 3. Ejecución de la Operación Principal (Consumo APESEG)
                var (datos, logRetornado) = await _apesegService.Apeseg_Anular(_param);

                if (logRetornado != null)
                {
                    apesegLog = logRetornado;
                    apesegLog.TramaEnvio = Newtonsoft.Json.JsonConvert.SerializeObject(_param);
                }

                if (!datos.OperacionExitosa)
                {
                    var errorsApeseg = datos.MensajeError ?? new List<string> { $"{datos.CodigoError}" };
                    respuestaFinal = new { success = false, message = $"ERROR APESEG: Error en anulación certificado {_param.PolizaCertificado}", data = (object)null, errors = errorsApeseg };
                    throw new Exception("Error devuelto por la plataforma externa APESEG.");
                }

                // 4. Flujo de Respuesta Exitosa
                Log.save(this, "EMPIEZA PROCESO DE GRABARLOG APESEG CERTIFICADO= " + _param.PolizaCertificado);

                respuestaFinal = new { success = true, message = "OK", data = datos, errors = (object)null };
                apesegLog.TramaRespuesta = Newtonsoft.Json.JsonConvert.SerializeObject(respuestaFinal);

                await Apeseg_Insertar(apesegLog);

                Log.save(this, "TERMINA PROCESO DE GRABARLOG APESEG CERTIFICADO= " + _param.PolizaCertificado);
                Log.save(this, "TERMINA ANULACIÓN A APESEG CERTIFICADO= " + _param.PolizaCertificado);

                return Ok(respuestaFinal);
            }
            catch (Exception ex)
            {
                Log.save(this, "ERROR: " + ex.Message);

                if (apesegLog == null)
                {
                    apesegLog = new ApesegLog
                    {
                        Tipo = "D",
                        Nro = _param?.PolizaCertificado,
                        Digito = -1,
                        User = _param?.UsuarioRegistro ?? "Desconocido",
                        Proveedor = _param?.Proveedor,
                        Canal = _param?.Canal,
                        PuntoVenta = _param?.PuntoVenta,
                        IpCliente = ipCliente,
                        TramaEnvio = _param != null ? Newtonsoft.Json.JsonConvert.SerializeObject(_param) : "Request Body Nulo",
                        Envia = "",
                        Recibe = ""
                    };
                }

                if (respuestaFinal == null)
                {
                    respuestaFinal = new { success = false, message = $"ERROR API: Error en anulación certificado {_param?.PolizaCertificado}", data = (object)null, errors = new List<string> { ex.Message } };
                }

                apesegLog.Err = string.IsNullOrEmpty(apesegLog.Err) ? ex.Message : apesegLog.Err ;
                apesegLog.TramaRespuesta = Newtonsoft.Json.JsonConvert.SerializeObject(respuestaFinal);

                await Apeseg_Insertar(apesegLog);

                return BadRequest(respuestaFinal);
            }
        }

        [HttpPost]
        [Route("Consultar")]
        public async Task<IActionResult> Consultar([FromBody] ConsultaSOATRequest _param)
        {
            string ipCliente = GetClientIp();
            ApesegLog apesegLog = null;
            object respuestaFinal = null;

            try
            {
                // 1. Validación de Request (ModelState)
                if (!ModelState.IsValid)
                {
                    var modelErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    respuestaFinal = new { success = false, message = "ERROR: Mala petición (Modelo inválido)", data = (object)null, errors = modelErrors };

                    throw new Exception("Fallo de validación en ModelState: " + string.Join(" | ", modelErrors));
                }

                _param.IpCliente = ipCliente;

                apesegLog = new ApesegLog
                {
                    Tipo = "C",
                    Nro = null,
                    Digito = -1,
                    User = _param.UsuarioRegistro,
                    Proveedor = _param.Proveedor,
                    Canal = _param.Canal,
                    PuntoVenta = _param.PuntoVenta,
                    IpCliente = _param.IpCliente,
                    TramaEnvio = Newtonsoft.Json.JsonConvert.SerializeObject(_param),
                    Envia = "",
                    Recibe = ""
                };

                Log.saveFirstLine();
                Log.save(this, "EMPIEZA Consultar A APESEG / _placa= " + _param.placa);

                // 2. Validación de Reglas de Negocio Locales
                Log.save(this, "EMPIEZA VALIDACION API CONSULTA PLACA= " + _param.placa);
                var errores = await _apesegService.Apeseg_Validar("C", _param);
                if (errores.Any())
                {
                    respuestaFinal = new { success = false, message = $"ERROR API: Error en consulta de placa {_param.placa}", data = (object)null, errors = errores };
                    throw new Exception("Fallo en validación de reglas de negocio locales (Consulta).");
                }
                Log.save(this, "TERMINA VALIDACION API CONSULTA PLACA= " + _param.placa);

                // 3. Ejecución de la Operación Principal (Consumo APESEG)
                var (datos, logRetornado) = await _apesegService.Consultar(_param);

                if (logRetornado != null)
                {
                    apesegLog = logRetornado;
                    apesegLog.TramaEnvio = Newtonsoft.Json.JsonConvert.SerializeObject(_param);
                }

                Log.save(this, "TERMINA Consultar A APESEG / _placa= " + _param.placa);

                if (datos is null)
                {
                    respuestaFinal = new { success = false, message = "ERROR: No se obtuvo respuesta de APESEG", data = (object)null, errors = new List<string> { "La respuesta de consulta externa retornó un valor nulo." } };
                    throw new Exception("La respuesta del servicio externo (Consultar) devolvió una entidad nula.");
                }

                // 4. Formatear salida de certificado
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

                // 5. Flujo de Respuesta Exitosa
                Log.save(this, "EMPIEZA PROCESO DE GRABARLOG APESEG PLACA= " + _param.placa);

                respuestaFinal = new { success = true, message = "OK", data = datos, errors = (object)null };
                apesegLog.TramaRespuesta = Newtonsoft.Json.JsonConvert.SerializeObject(respuestaFinal);

                await Apeseg_Insertar(apesegLog);

                Log.save(this, "TERMINA PROCESO DE GRABARLOG APESEG PLACA= " + _param.placa);

                return Ok(respuestaFinal);
            }
            catch (Exception ex)
            {
                Log.save(this, "ERROR: " + ex.Message);

                if (apesegLog == null)
                {
                    apesegLog = new ApesegLog
                    {
                        Tipo = "C",
                        Nro = null,
                        Digito = -1,
                        User = _param?.UsuarioRegistro ?? "Desconocido",
                        Proveedor = _param?.Proveedor,
                        Canal = _param?.Canal,
                        PuntoVenta = _param?.PuntoVenta,
                        IpCliente = ipCliente,
                        TramaEnvio = _param != null ? Newtonsoft.Json.JsonConvert.SerializeObject(_param) : "Request Body Nulo",
                        Envia = "",
                        Recibe = ""
                    };
                }

                if (respuestaFinal == null)
                {
                    respuestaFinal = new { success = false, message = $"ERROR API: Error al consultar la placa {_param?.placa}", data = (object)null, errors = new List<string> { ex.Message } };
                }

                apesegLog.Err = string.IsNullOrEmpty(apesegLog.Err) ? ex.Message : apesegLog.Err ;
                apesegLog.TramaRespuesta = Newtonsoft.Json.JsonConvert.SerializeObject(respuestaFinal);

                await Apeseg_Insertar(apesegLog);

                return BadRequest(respuestaFinal);
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

        private async Task Apeseg_Insertar(ApesegLog log)
        {
            try
            {
                if (log != null)
                {
                    await _apesegService.Apeseg_Insertar(log);
                }
            }
            catch (Exception exLog)
            {
                Log.save(this, "CRÍTICO CONTROLLER: Falló la inserción del log transaccional en BD: " + exLog.Message);
            }
        }
        #endregion
    }
}