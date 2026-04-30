using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using VidaCamara.CrossCuting.Utilities;
using VidaCamara.CrossCuting.Utilities.Configuration;
using VidaCamara.Domain.Services.Entidades.Apeseg;
using VidaCamara.Domain.Services.Repositorios.Apeseg;
using VidaCamara.Domain.Services.Repositorios.Helper;
using VidaCamara.Infrastructure.Connection;

namespace VidaCamara.Infrastructure.Data.Apeseg
{
    public class ApesegRepository : IApesegRepository
    {
        private readonly IOptions<AppSettings> _appSettings;
        private readonly IConnectionBase _connectionBase;
        private readonly IHelperRepository _helperRepository;

        public ApesegRepository(IOptions<AppSettings> appSettings, IConnectionBase connectionBase, IHelperRepository helperRepository)
        {
            _appSettings = appSettings;
            _connectionBase = connectionBase;
            _helperRepository = helperRepository;
        }

        #region MÉTODOS API (PÚBLICOS)

        public async Task<RegistrarResponse> Apeseg_Registrar(RegistrarParam param)
        {
            string jsonEnvia = string.Empty;
            try
            {
                string token = await ObtenerToken();
                string subKey = await _helperRepository.GetValorTablaConfig("APESEG.SubscriptionKey");
                string uri = await _helperRepository.GetValorTablaConfig("APESEG.UriRegistrarSOAT");
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

                    GrabarLog("R", param.PolizaCertificado, digito, status, jsonEnvia, jsonRecibe, param.UsuarioCreacion);
                    return resultado;
                }
            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(0)?.GetFileLineNumber() ?? 0;
                Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
                GrabarLog("R", param.PolizaCertificado, -1, "Error Registrar: " + ex.Message, jsonEnvia, "", param.UsuarioCreacion);
                throw;
            }
        }

        public async Task<ModificarResponse> Apeseg_Actualizar(ModificarParam param)
        {
            string jsonEnvia = string.Empty;
            try
            {
                string token = await ObtenerToken();
                string subKey = await _helperRepository.GetValorTablaConfig("APESEG.SubscriptionKey");
                string uri = await _helperRepository.GetValorTablaConfig("APESEG.UriActualizarSOAT");
                jsonEnvia = JsonConvert.SerializeObject(param);

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subKey);

                    var response = await client.PutAsync(uri, new StringContent(jsonEnvia, Encoding.UTF8, "application/json"));
                    var jsonRecibe = await response.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<ModificarResponse>(jsonRecibe);

                    string status = resultado.OperacionExitosa ? "OK" : string.Join(",", resultado.CodigoError);

                    GrabarLog("U", param.PolizaCertificado, int.Parse(param.DigitoVerificador), status, jsonEnvia, jsonRecibe, param.UsuarioModificacion);
                    return resultado;
                }
            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(0)?.GetFileLineNumber() ?? 0;
                Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
                GrabarLog("U", param.PolizaCertificado, -1, "Error Modificar: " + ex.Message, jsonEnvia, "", param.UsuarioModificacion);
                throw;
            }
        }

        public async Task<AnularResponse> Apeseg_Anular(AnularParam param)
        {
            string jsonEnvia = string.Empty;
            try
            {
                string token = await ObtenerToken();
                string subKey = await _helperRepository.GetValorTablaConfig("APESEG.SubscriptionKey");
                string uri = await _helperRepository.GetValorTablaConfig("APESEG.UriAnularSOAT");
                jsonEnvia = JsonConvert.SerializeObject(param);

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subKey);

                    var response = await client.PutAsync(uri, new StringContent(jsonEnvia, Encoding.UTF8, "application/json"));
                    var jsonRecibe = await response.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<AnularResponse>(jsonRecibe);

                    string status = resultado.OperacionExitosa ? "OK" : string.Join(",", resultado.CodigoError);

                    GrabarLog("D", param.polizaCertificado, int.Parse(param.digitoVerificador), status, jsonEnvia, jsonRecibe, param.usuarioModificacion);
                    return resultado;
                }
            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(0)?.GetFileLineNumber() ?? 0;
                Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
                GrabarLog("D", param.polizaCertificado, -1, "Error Anular: " + ex.Message, jsonEnvia, "", param.usuarioModificacion);
                throw;
            }
        }

        public async Task<ConsultarResponse> Consultar(ConsultarParam param)
        {
            ConsultarResponse respuestaServicio = new ConsultarResponse();
            try
            {
                var subKey = param.subscriptionKey
                             ?? await _helperRepository.GetValorTablaConfig("APESEG.SubscriptionKey");

                string uriBase = await _helperRepository.GetValorTablaConfig("APESEG.UriConsultarSOAT") + param.placa;

                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(int.TryParse(_helperRepository.GetValorTablaConfig("APESEG.TiempoRespuesta")?.ToString(), out int s) ? s : 30);
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

        #endregion

        #region MÉTODOS PRIVADOS (INTERNAL LOGIC)

        private async Task<string> ObtenerToken()
        {
            try
            { 
                string aadInstance = (string)_connectionBase.ExecuteScalarSqlFunction("dbo.fnc_TablaConfiguracion_valor", new[] { "APESEG.AADInstance", "0" });
                string tenant = (string)_connectionBase.ExecuteScalarSqlFunction("dbo.fnc_TablaConfiguracion_valor", new[] { "APESEG.Tenant", "0" });

                string endpoint = string.Format(_appSettings.Value.AADInstance, _appSettings.Value.Tenant);
                string clientId = (string)_connectionBase.ExecuteScalarSqlFunction("dbo.fnc_TablaConfiguracion_valor", new[] { "APESEG.ClientId", "0" });
                string clientSecret = (string)_connectionBase.ExecuteScalarSqlFunction("dbo.fnc_TablaConfiguracion_valor", new[] { "APESEG.ClientSecret", "0" });
                string scope = (string)_connectionBase.ExecuteScalarSqlFunction("dbo.fnc_TablaConfiguracion_valor", new[] { "APESEG.ScopeAudience", "0" });

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

        private void GrabarLog(string tipo, string nro, int digito, string err, string envia, string recibe, string user)
        {
            List<SqlParameter> parameters = new List<SqlParameter> {
                new SqlParameter("@p_TipoEnvio", tipo),
                new SqlParameter("@p_NumCertificado", nro ?? ""),
                new SqlParameter("@p_Digito", digito),
                new SqlParameter("@p_Error", err ?? ""),
                new SqlParameter("@p_Enviado", envia ?? ""),
                new SqlParameter("@p_Recibido", recibe ?? ""),
                new SqlParameter("@p_Usuario", user ?? "SYS")
            };
            try
            {
                _connectionBase.ExecuteByStoredProcedure("[sp_Apeseg_INS]", parameters, ConnectionBase.enuTypeDataBase.sqlCon);
            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(0)?.GetFileLineNumber() ?? 0;
                Log.save(this, "ERROR GRABAR LOG EN LINEA: " + line + " / " + ex.Message);
            }
        }

        #endregion
    }
}