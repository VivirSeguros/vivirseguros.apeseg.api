using AutoMapper;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VidaCamara.CrossCuting.Log.Contracts;
using VidaCamara.CrossCuting.Utilities;
using VidaCamara.CrossCuting.Utilities.Configuration;
using VidaCamara.Domain.Services.Entidades.Apeseg;
using VidaCamara.Domain.Services.Repositorios.Apeseg;
using VidaCamara.Domain.Services.Repositorios.Helper;
using VidaCamara.Masivos.Services.Services.Helper;

namespace VidaCamara.Masivos.Services.Services.Apeseg
{
    public class ApesegService : IApesegService
    {
        private readonly IHelperService _helperService;
        private readonly IApesegRepository _apesegRepository;

        public ApesegService(IApesegRepository apesegRepository, IHelperService helperService)
        {
            _apesegRepository = apesegRepository;
            _helperService = helperService;
        }

        public async Task<RegistrarResponse> Apeseg_Registrar(RegistroSOATRequest request)
        {
            RegistrarParam param = new RegistrarParam
            {
                CodigoAseguradora = request.CodigoAseguradora,
                PolizaCertificado = request.PolizaCertificado,
                FechaInicioVigencia = request.FechaInicioVigencia,
                FechaFinVigencia = request.FechaFinVigencia,
                CodigoTipoPersona = request.CodigoTipoPersona,
                NombreContratante = request.NombreContratante,
                CodigoTipoDocumento = request.CodigoTipoDocumento,
                NumeroDocumento = request.NumeroDocumento,
                PlacaVehiculo = request.PlacaVehiculo,
                CodigoUsoVehiculo = request.CodigoUsoVehiculo,
                CodigoClaseVehiculo = request.CodigoClaseVehiculo,
                PaisPlaca = request.PaisPlaca,
                FechaIngreso = DateTime.Now.ToString("dd/MM/yyyy"), // request.FechaRegistro,
                CodigoUbigeo = request.CodigoUbigeo,
                NumeroSerieMotor = request.NumeroSerieMotor,
                FechaControlPolicial = request.FechaControlPolicial,
                TipoCertificado = request.TipoCertificado,
                TelefonoContacto = request.TelefonoContacto,
                CorreoContacto = request.CorreoContacto,
                UsuarioCreacion = request.UsuarioRegistro,
                Marca = request.Marca,
                NumeroAsientos = request.NumeroAsientos,
                ModeloVehiculo = request.ModeloVehiculo
            };

            string jsonEnvia = string.Empty;
            try
            {
                string token = await ObtenerToken();
                string subKey = await _helperService.GetValorTablaConfig("APESEG.SubscriptionKey");
                string uri = await _helperService.GetValorTablaConfig("APESEG.UriRegistrarSOAT");
                jsonEnvia = JsonConvert.SerializeObject(param);

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subKey);

                    var response = await client.PostAsync(uri, new StringContent(jsonEnvia, Encoding.UTF8, "application/json"));
                    var jsonRecibe = await response.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<RegistrarResponse>(jsonRecibe);

                    string status = resultado.OperacionExitosa ? "OK" : string.Join(",", resultado.CodigoError);
                    int digito = resultado.OperacionExitosa ? resultado.DigitoVerificador : -1;

                    ApesegLog apesegLog = new ApesegLog
                    {
                        Tipo = "R",
                        Nro = param.PolizaCertificado,
                        Digito = digito,
                        Err = status,
                        Envia = jsonEnvia,
                        Recibe = jsonRecibe,
                        User = param.UsuarioCreacion,
                        Proveedor = request.Proveedor,
                        Canal = request.Canal,
                        PuntoVenta = request.PuntoVenta,
                        IpCliente = request.IpCliente,
                    };

                    _apesegRepository.GrabarLog(apesegLog);
                    return resultado;
                }
            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(0)?.GetFileLineNumber() ?? 0;
                Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
                ApesegLog apesegLog = new ApesegLog
                {
                    Tipo = "R",
                    Nro = param.PolizaCertificado,
                    Digito = -1,
                    Err = "Error Registrar: " + ex.Message,
                    Envia = jsonEnvia,
                    Recibe = "",
                    User = param.UsuarioCreacion,
                    Proveedor = request.Proveedor,
                    Canal = request.Canal,
                    PuntoVenta = request.PuntoVenta,
                    IpCliente = request.IpCliente,
                };
                _apesegRepository.GrabarLog(apesegLog);
                throw;
            }
        }

        public async Task<ModificarResponse> Apeseg_Actualizar(ModificarSOATRequest request)
        {
            string jsonEnvia = string.Empty;

            ModificarParam param = new ModificarParam
            {
                CodigoAseguradora = request.CodigoAseguradora,
                PolizaCertificado = request.PolizaCertificado,
                DigitoVerificador = request.DigitoVerificador,
                FechaInicioVigencia = request.FechaInicioVigencia,
                FechaFinVigencia = request.FechaFinVigencia,
                CodigoTipoPersona = request.CodigoTipoPersona,
                NombreContratante = request.NombreContratante,
                CodigoTipoDocumento = request.CodigoTipoDocumento,
                NumeroDocumento = request.NumeroDocumento,
                PlacaVehiculo = request.PlacaVehiculo,
                CodigoUsoVehiculo = request.CodigoUsoVehiculo,
                CodigoClaseVehiculo = request.CodigoClaseVehiculo,
                PaisPlaca = request.PaisPlaca,
                FechaActualizacion = DateTime.Now.ToString("dd/MM/yyyy"),
                CodigoUbigeo = request.CodigoUbigeo,
                NumeroSerieMotor = request.NumeroSerieMotor,
                NumeroSerieChasis = request.NumeroSerieChasis,
                FechaControlPolicial = request.FechaControlPolicial,
                TipoCertificado = request.TipoCertificado,
                UsuarioModificacion = request.UsuarioRegistro,
                Marca = request.Marca,
                NumeroAsientos = request.NumeroAsientos,
                ModeloVehiculo = request.ModeloVehiculo
            };

            try
            {
                string token = await ObtenerToken();
                string subKey = await _helperService.GetValorTablaConfig("APESEG.SubscriptionKey");
                string uri = await _helperService.GetValorTablaConfig("APESEG.UriActualizarSOAT");
                jsonEnvia = JsonConvert.SerializeObject(param);

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subKey);

                    var response = await client.PutAsync(uri, new StringContent(jsonEnvia, Encoding.UTF8, "application/json"));
                    var jsonRecibe = await response.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<ModificarResponse>(jsonRecibe);

                    string status = resultado.OperacionExitosa ? "OK" : string.Join(",", resultado.CodigoError);

                    ApesegLog apesegLog = new ApesegLog
                    {
                        Tipo = "U",
                        Nro = param.PolizaCertificado,
                        Digito = int.Parse(param.DigitoVerificador),
                        Err = status,
                        Envia = jsonEnvia,
                        Recibe = jsonRecibe,
                        User = param.UsuarioModificacion,
                        Proveedor = request.Proveedor,
                        Canal = request.Canal,
                        PuntoVenta = request.PuntoVenta,
                        IpCliente = request.IpCliente,
                    };
                    
                    _apesegRepository.GrabarLog(apesegLog);
                    return resultado;
                }
            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(0)?.GetFileLineNumber() ?? 0;
                Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);

                ApesegLog apesegLog = new ApesegLog
                {
                    Tipo = "U",
                    Nro = param.PolizaCertificado,
                    Digito = -1,
                    Err = "Error Modificar: " + ex.Message,
                    Envia = jsonEnvia,
                    Recibe = "",
                    User = param.UsuarioModificacion,
                    Proveedor = request.Proveedor,
                    Canal = request.Canal,
                    PuntoVenta = request.PuntoVenta,
                    IpCliente = request.IpCliente,
                };
                _apesegRepository.GrabarLog(apesegLog);
                throw;
            }
        }

        public async Task<AnularResponse> Apeseg_Anular(AnulacionSOATRequest request)
        {
            string jsonEnvia = string.Empty;

            AnularParam param = new AnularParam
            {
                codigoAseguradora = request.CodigoAseguradora,
                polizaCertificado = request.PolizaCertificado,
                digitoVerificador = request.DigitoVerificador,
                codigoTipoAnulacion = request.CodigoTipoAnulacion,
                fechaAnulacion = DateTime.Now.ToString("dd/MM/yyyy"),
                usuarioModificacion = request.UsuarioRegistro
            };

            try
            {
                string token = await ObtenerToken();
                string subKey = await _helperService.GetValorTablaConfig("APESEG.SubscriptionKey");
                string uri = await _helperService.GetValorTablaConfig("APESEG.UriAnularSOAT");
                jsonEnvia = JsonConvert.SerializeObject(param);

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subKey);

                    var response = await client.PutAsync(uri, new StringContent(jsonEnvia, Encoding.UTF8, "application/json"));
                    var jsonRecibe = await response.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<AnularResponse>(jsonRecibe);

                    string status = resultado.OperacionExitosa ? "OK" : string.Join(",", resultado.CodigoError);

                    ApesegLog apesegLog = new ApesegLog
                    {
                        Tipo = "D",
                        Nro = param.polizaCertificado,
                        Digito = int.Parse(param.digitoVerificador),
                        Err = status,
                        Envia = jsonEnvia,
                        Recibe = jsonRecibe,
                        User = param.usuarioModificacion,
                        Proveedor = request.Proveedor,
                        Canal = request.Canal,
                        PuntoVenta = request.PuntoVenta,
                        IpCliente = request.IpCliente,
                    };

                    _apesegRepository.GrabarLog(apesegLog);

                    return resultado;
                }
            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(0)?.GetFileLineNumber() ?? 0;
                Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
                ApesegLog apesegLog = new ApesegLog
                {
                    Tipo = "D",
                    Nro = param.polizaCertificado,
                    Digito = -1,
                    Err = "Error Anular: " + ex.Message,
                    Envia = jsonEnvia,
                    Recibe = "",
                    User = param.usuarioModificacion,
                    Proveedor = request.Proveedor,
                    Canal = request.Canal,
                    PuntoVenta = request.PuntoVenta,
                    IpCliente = request.IpCliente,

                };
                _apesegRepository.GrabarLog(apesegLog);
                throw;
            }
        }

        public async Task<ConsultarResponse> Consultar(ConsultaSOATRequest request)
        {
            ConsultarParam param = new ConsultarParam
            {
                placa = request.placa,
                subscriptionKey = request.subscriptionKey
            };


            ConsultarResponse respuestaServicio = new ConsultarResponse();
            try
            {
                var subKey = param.subscriptionKey
                             ?? await _helperService.GetValorTablaConfig("APESEG.SubscriptionKey");

                string uriBase = await _helperService.GetValorTablaConfig("APESEG.UriConsultarSOAT") + param.placa;

                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(int.TryParse(_helperService.GetValorTablaConfig("APESEG.TiempoRespuesta")?.ToString(), out int s) ? s : 30);
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subKey);

                    Log.save(this, "EMPIEZA INVOCACIÓN SERVICIO CONSULTA APESEG / placa=" + param.placa);

                    HttpResponseMessage response = await client.GetAsync(uriBase);
                    string respuestaJson = await response.Content.ReadAsStringAsync();

                    respuestaServicio = JsonConvert.DeserializeObject<ConsultarResponse>(respuestaJson);

                    Log.save(this, "TERMINA INVOCACIÓN SERVICIO CONSULTA APESEG / placa=" + param.placa);
                }
            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(0)?.GetFileLineNumber() ?? 0;
                Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
            }

            return respuestaServicio;
        }



        #region PRIVATE METHODS
        private async Task<string> ObtenerToken()
        {
            try
            {
                string aadInstance = await _helperService.GetValorTablaConfig("APESEG.AADInstance");
                string tenant = await _helperService.GetValorTablaConfig("APESEG.Tenant"); 
                string endpoint = string.Format(aadInstance, tenant);
                string clientId = await _helperService.GetValorTablaConfig("APESEG.ClientId");
                string clientSecret = await _helperService.GetValorTablaConfig("APESEG.ClientSecret");
                string scope = await _helperService.GetValorTablaConfig("APESEG.ScopeAudience");

                var form = new Dictionary<string, string>
                {
                    ["grant_type"] = "client_credentials",
                    ["client_id"] = clientId,
                    ["client_secret"] = clientSecret,
                    ["scope"] = scope
                };

                using (var http = new HttpClient())
                {
                    var resp = await http.PostAsync(endpoint, new FormUrlEncodedContent(form));
                    var json = await resp.Content.ReadAsStringAsync();
                    dynamic data = JsonConvert.DeserializeObject(json);
                    return data.access_token;
                }
            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(0)?.GetFileLineNumber() ?? 0;
                Log.save(this, "ERROR TOKEN EN LINEA: " + line + " / " + ex.Message);
                throw;
            }
        }



        #endregion


    }
}