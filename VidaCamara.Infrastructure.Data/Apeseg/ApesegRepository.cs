using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Net;
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
        private readonly IOptions<AppSettings> appSettings;
        private readonly IConnectionBase _connectionBase;

        private static HttpClient httpClient = new HttpClient();
        private static AuthenticationContext contextoAutenticacion = null;
        private static ClientCredential credencialesCliente = null;
        private readonly IHelperRepository _helperRepository;

        public ApesegRepository(IOptions<AppSettings> appSettings,
                               IConnectionBase ConnectionBase, IHelperRepository helperRepository)
        {
            this.appSettings = appSettings;
            _connectionBase = ConnectionBase;
            _helperRepository = helperRepository;
        }

        public async Task<Registrar> Apeseg_Send(ApesegParam param)
        {
            Registrar result = null;

            var vehiculoR = new RegistrarParam();
            var vehiculoM = new ModificarParam();
            var vehiculoA = new AnularParam();

            // Crear JSON
            List<SqlParameter> parameters = new List<SqlParameter> {
                   new SqlParameter("@p_Auto", param.idAuto),
            };
            try
            {
                Log.save(this, "EMPIEZA OBTENCIÓN DE DATOS POR AUTOID / STOREPROCEDURE sp_Apeseg_Auto_SEL / AUTOID= " + param.idAuto);
                using (SqlDataReader dr = (SqlDataReader)_connectionBase.ExecuteByStoredProcedure("sp_Apeseg_Auto_SEL",
                           parameters, ConnectionBase.enuTypeDataBase.sqlCon))
                {
                    dr.Read();
                    vehiculoR.CodigoAseguradora = dr.GetString(dr.GetOrdinal("Aseguradora"));
                    vehiculoR.PolizaCertificado = dr.GetString(dr.GetOrdinal("NumCertif"));
                    vehiculoR.FechaInicioVigencia = dr.GetString(dr.GetOrdinal("FechaInicio"));
                    vehiculoR.FechaFinVigencia = dr.GetString(dr.GetOrdinal("FechaFin"));
                    vehiculoR.CodigoTipoPersona = dr.GetString(dr.GetOrdinal("TipoPersona"));
                    vehiculoR.NombreContratante = dr.GetString(dr.GetOrdinal("Contratante"));
                    vehiculoR.CodigoTipoDocumento = dr.GetString(dr.GetOrdinal("TipoDoc"));
                    vehiculoR.NumeroDocumento = dr.GetString(dr.GetOrdinal("NroDocumento"));
                    vehiculoR.PlacaVehiculo = dr.GetString(dr.GetOrdinal("Placa"));
                    vehiculoR.CodigoUsoVehiculo = dr.GetString(dr.GetOrdinal("CodUso"));
                    vehiculoR.CodigoClaseVehiculo = dr.GetString(dr.GetOrdinal("CodClase"));
                    vehiculoR.PaisPlaca = "0001";
                    vehiculoR.FechaIngreso = dr.GetString(dr.GetOrdinal("FechaInicio"));
                    vehiculoR.CodigoUbigeo = dr.GetString(dr.GetOrdinal("Ubigeo"));
                    vehiculoR.NumeroSerieMotor = dr.GetString(dr.GetOrdinal("Serie"));
                    vehiculoR.FechaControlPolicial = dr.GetString(dr.GetOrdinal("FechaInicio"));
                    vehiculoR.TipoCertificado = dr.GetString(dr.GetOrdinal("Tipo"));
                    vehiculoR.TelefonoContacto = dr.GetString(dr.GetOrdinal("Celular"));
                    vehiculoR.CorreoContacto = dr.GetString(dr.GetOrdinal("Email"));
                    vehiculoR.UsuarioCreacion = dr.GetString(dr.GetOrdinal("UsuarioCreacion"));
                    vehiculoR.Marca = dr.GetString(dr.GetOrdinal("Marca"));
                    vehiculoR.NumeroAsientos = dr.GetInt32(dr.GetOrdinal("Asientos")).ToString();
                    vehiculoR.ModeloVehiculo = dr.GetString(dr.GetOrdinal("Modelo"));

                }
                // Para Modificar
                vehiculoM.CodigoAseguradora = vehiculoR.CodigoAseguradora;
                vehiculoM.PolizaCertificado = vehiculoR.PolizaCertificado;
                vehiculoM.DigitoVerificador = param.Digito.ToString();
                vehiculoM.FechaInicioVigencia = vehiculoR.FechaInicioVigencia;
                vehiculoM.FechaFinVigencia = vehiculoR.FechaFinVigencia;
                vehiculoM.CodigoTipoPersona = vehiculoR.CodigoTipoPersona;
                vehiculoM.NombreContratante = vehiculoR.NombreContratante;
                vehiculoM.CodigoTipoDocumento = vehiculoR.CodigoTipoDocumento;
                vehiculoM.NumeroDocumento = vehiculoR.NumeroDocumento;
                vehiculoM.PlacaVehiculo = vehiculoR.PlacaVehiculo;
                vehiculoM.CodigoUsoVehiculo = vehiculoR.CodigoUsoVehiculo;
                vehiculoM.CodigoClaseVehiculo = vehiculoR.CodigoClaseVehiculo;
                vehiculoM.PaisPlaca = vehiculoR.PaisPlaca;
                vehiculoM.FechaActualizacion = DateTime.Now.ToString("dd/MM/yyyy");
                vehiculoM.CodigoUbigeo = vehiculoR.CodigoUbigeo;
                vehiculoM.NumeroSerieMotor = vehiculoR.NumeroSerieMotor;
                vehiculoM.NumeroSerieChasis = vehiculoR.NumeroSerieChasis;
                vehiculoM.FechaControlPolicial = vehiculoR.FechaControlPolicial;
                vehiculoM.TipoCertificado = vehiculoR.TipoCertificado;
                vehiculoM.UsuarioModificacion = vehiculoR.UsuarioCreacion;
                vehiculoM.Marca = vehiculoR.Marca;
                vehiculoM.NumeroAsientos = vehiculoR.NumeroAsientos;
                vehiculoM.ModeloVehiculo = vehiculoR.ModeloVehiculo;

                // Para Anular
                vehiculoA.codigoAseguradora = vehiculoR.CodigoAseguradora;
                vehiculoA.polizaCertificado = vehiculoR.PolizaCertificado;
                vehiculoA.digitoVerificador = vehiculoM.DigitoVerificador;
                vehiculoA.fechaAnulacion = DateTime.Now.ToString("dd/MM/yyyy");
                vehiculoA.codigoTipoAnulacion = param.TipoAnulacion;
                vehiculoA.usuarioModificacion = vehiculoR.UsuarioCreacion;
                Log.save(this, "TERMINA OBTENCIÓN DE DATOS POR AUTOID / STOREPROCEDURE sp_Apeseg_Auto_SEL / AUTOID= " + param.idAuto);
            }
            catch (Exception ex)
            {
                Log.save(this, ex.Message + " IDAUTO=" + param.idAuto);
                GrabarLog(param.Accion, vehiculoR.PolizaCertificado, -1, "Error al generar json envio", "", "", vehiculoR.UsuarioCreacion);
                throw ex;   // result = null;
            }

            // Accion solicitada
            var clienteRegistro = new HttpClient();
            string uri = ""; string jsonRegistro = ""; string respuesta = ""; int digito = 0; string status = "";
            try
            {
                Log.save(this, "EMPIEZA OBTENER TOKEN / AUTOID=" + param.idAuto);
                var token = await ObtenerToken();
                Log.save(this, "TERMINA OBTENER TOKEN / AUTOID=" + param.idAuto);
                clienteRegistro.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                //clienteRegistro.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", appSettings.Value.SubscriptionKey);
                string APESEG_SubscriptionKey = (string)_connectionBase.ExecuteScalarSqlFunction("dbo.fnc_TablaConfiguracion_valor", new string[] { "APESEG.SubscriptionKey", "0" });
                clienteRegistro.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", APESEG_SubscriptionKey);
                clienteRegistro.DefaultRequestHeaders.Add("Authorization", token);

                var UriRegistrarSOAT =  (_helperRepository.GetValorTablaConfig("APESEG.UriRegistrarSOAT")).ToString(); 
                var UriActualizarSOAT = (_helperRepository.GetValorTablaConfig("APESEG.UriActualizarSOAT")).ToString(); 
                var UriAnularSOAT = (_helperRepository.GetValorTablaConfig("APESEG.UriAnularSOAT")).ToString(); 

                switch (param.Accion)
                {
                    case "R":
                        jsonRegistro = JsonConvert.SerializeObject(UriRegistrarSOAT);
                        break;
                    case "U":
                        jsonRegistro = JsonConvert.SerializeObject(UriActualizarSOAT);
                        break;
                    case "D":
                        jsonRegistro = JsonConvert.SerializeObject(UriAnularSOAT);
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.save(this, ex.Message + "ERROR AL OBTENER TOKEN / AUTOID=" + param.idAuto);
                GrabarLog(param.Accion, vehiculoR.PolizaCertificado, -1, "Error del Servicio", jsonRegistro, "", vehiculoR.UsuarioCreacion);
                throw ex;   // result = null;
            }

            HttpResponseMessage response;
            byte[] byteData = Encoding.UTF8.GetBytes(jsonRegistro);

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                Log.save(this, "EMPIEZA INVOCACIÓN SERVICIO APESEG / AUTOID=" + param.idAuto);
                switch (param.Accion)
                {
                    case "R":
                        response = await clienteRegistro.PostAsync(uri, content);
                        respuesta = await response.Content.ReadAsStringAsync();
                        Registrar servicioR = JsonConvert.DeserializeObject<Registrar>(respuesta);
                        digito = servicioR.OperacionExitosa ? servicioR.DigitoVerificador : -1;
                        status = servicioR.OperacionExitosa ? "OK" : string.Join(",", servicioR.CodigoError);
                        break;
                    case "U":
                        response = await clienteRegistro.PutAsync(uri, content);
                        respuesta = await response.Content.ReadAsStringAsync();
                        Modificar servicioM = JsonConvert.DeserializeObject<Modificar>(respuesta);
                        digito = param.Digito; //servicioM.OperacionExitosa ? param.Digito : -1;
                        status = servicioM.OperacionExitosa ? "OK" : string.Join(",", servicioM.CodigoError);
                        break;
                    case "D":
                        response = await clienteRegistro.PutAsync(uri, content);
                        respuesta = await response.Content.ReadAsStringAsync();
                        Anular servicioA = JsonConvert.DeserializeObject<Anular>(respuesta);
                        digito = param.Digito; //servicioA.OperacionExitosa ? param.Digito : -1;
                        status = servicioA.OperacionExitosa ? "OK" : string.Join(",", servicioA.CodigoError);
                        break;
                }
                Log.save(this, "TERMINA INVOCACIÓN SERVICIO APESEG / AUTOID=" + param.idAuto);
            }

            // Grabar el resultado
            GrabarLog(param.Accion, vehiculoR.PolizaCertificado, digito, status, jsonRegistro, respuesta, vehiculoR.UsuarioCreacion);

            return JsonConvert.DeserializeObject<Registrar>(respuesta); ;
        }


        public async Task<Consultar> Consultar(string placa, string subscriptionKey = null)
        {
            var clienteRegistro = new HttpClient();
            Consultar respuestaServicio = new Consultar();
            try
            {
                var subKey = subscriptionKey ?? await _helperRepository.GetValorTablaConfig("APESEG.SubscriptionKey");

                clienteRegistro.Timeout = TimeSpan.FromSeconds(int.Parse(UtilHelper.obtainConfig("tiempoRespuestaServiciosIC")));
                clienteRegistro.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                clienteRegistro.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subKey);

                string UriConsultarSOAT = (await _helperRepository.GetValorTablaConfig("APESEG.UriConsultarSOAT")).ToString();
                UriConsultarSOAT = UriConsultarSOAT + placa;

                Log.save(this, "EMPIEZA INVOCACIÓN SERVICIO CONSULTA APESEG / placa=" + placa);

                HttpResponseMessage response = await clienteRegistro.GetAsync(UriConsultarSOAT);
                string respuesta = await response.Content.ReadAsStringAsync();
                respuestaServicio = JsonConvert.DeserializeObject<Consultar>(respuesta);
                Log.save(this, "TERMINA INVOCACIÓN SERVICIO CONSULTA APESEG / placa=" + placa);
            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(3).GetFileLineNumber();
                Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
            }

            return respuestaServicio;
        }

        private string GrabarLog(string tipo, string nro, int digito, string err, string envia, string recibe, string user)
        {
            // Grabar el resultado
            Log.save(this, "EMPIEZA METODO GrabarLog PASE PARAMETROS A PROC dbo.sp_Apeseg_INS");
            List<SqlParameter> parameter2 = new List<SqlParameter> {
                new SqlParameter("@p_TipoEnvio", tipo),
                new SqlParameter("@p_NumCertificado", nro),
                new SqlParameter("@p_Digito", digito),
                new SqlParameter("@p_Error", err),
                new SqlParameter("@p_Enviado", envia),
                new SqlParameter("@p_Recibido", recibe),
                new SqlParameter("@p_Usuario", user)
            };
            try
            {
                Log.save(this, "PARAMETROS METODO GrabarLog: tipo: " + tipo + ", nro: " + nro + ", digito: " + digito + ", err:" +
                                                                       err + ", envia:" + envia + ", recibe:" + recibe + ", user:" + user);
                _connectionBase.ExecuteByStoredProcedure("[sp_Apeseg_INS]", parameter2, ConnectionBase.enuTypeDataBase.sqlCon);
            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(3).GetFileLineNumber();
                Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
                return "";
            }
            Log.save(this, "TERMINA METODO GrabarLog PASE PARAMETROS A PROC dbo.sp_Apeseg_INS");
            return "";
        }



        /// <summary> JSIzipay
        /// Método encargado de obtener el token de autorización desde el AD de Azure
        /// </summary>
        /// <returns>Cadena que representa el token de autorización para el consumo de las operaciones de Registro.</returns>

        private async Task<string> ObtenerToken()
        {
            string tokenAutorizacion = string.Empty;
            string tokenEndpoint = String.Format(CultureInfo.InvariantCulture, appSettings.Value.AADInstance, appSettings.Value.Tenant);

            try
            {
                string clientId = (string)_connectionBase.ExecuteScalarSqlFunction("dbo.fnc_TablaConfiguracion_valor", new[] { "APESEG.ClientId", "0" });
                string clientSecret = (string)_connectionBase.ExecuteScalarSqlFunction("dbo.fnc_TablaConfiguracion_valor", new[] { "APESEG.ClientSecret", "0" });
                string scopeAudience = (string)_connectionBase.ExecuteScalarSqlFunction("dbo.fnc_TablaConfiguracion_valor", new[] { "APESEG.ScopeAudience", "0" });

                var form = new Dictionary<string, string>
                {
                    ["grant_type"] = "client_credentials",
                    ["client_id"] = clientId,
                    ["client_secret"] = clientSecret,
                    ["scope"] = scopeAudience
                };

                var http = new HttpClient();
                var content = new FormUrlEncodedContent(form);

                // Reintentos simples (3) con backoff
                for (int intento = 1; intento <= 3; intento++)
                {
                    var resp = await http.PostAsync(tokenEndpoint, content);
                    if (resp.IsSuccessStatusCode)
                    {
                        var json = await resp.Content.ReadAsStringAsync();
                        var dto = JsonConvert.DeserializeObject<ApesegTokenResponse>(json);
                        tokenAutorizacion = $"{dto.access_token}";
                        return tokenAutorizacion;
                    }

                    // Si es 5xx/429 reintenta; si es 4xx lanza de frente
                    if (resp.StatusCode == (HttpStatusCode)429 || (int)resp.StatusCode >= 500)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(3 * intento));
                        continue;
                    }
                    var errorBody = await resp.Content.ReadAsStringAsync();
                    throw new Exception($"Error al obtener token: {(int)resp.StatusCode} {resp.ReasonPhrase}. {errorBody}");
                }

                throw new Exception("No se pudo obtener el token después de 3 reintentos.");
            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(0)?.GetFileLineNumber() ?? 0;
                Log.save(this, $"ERROR EN LINEA: {line} / {ex.Message}");
                throw;
            }
        }

        public async Task<Consultar> Pago_Save(Respuesta user)
        {
            JSIzipay rptaServicio = JsonConvert.DeserializeObject<JSIzipay>(user.formToken);
            // Grabar el resultado
            var rpta = new Consultar();
            Log.save(this, "EMPIEZA METODO Pago_Save PASE PARAMETROS A PROC dbo.sp_Izipay_INS");
            List<SqlParameter> parameter = new List<SqlParameter> {
                new SqlParameter("@p_shopId", rptaServicio.clientAnswer.shopId),
                new SqlParameter("@p_uuid", rptaServicio.clientAnswer.transactions[0].uuid),
                new SqlParameter("@p_currency", rptaServicio.clientAnswer.transactions[0].currency),
                new SqlParameter("@p_amount", rptaServicio.clientAnswer.transactions[0].amount),
                new SqlParameter("@p_OrderStatus", rptaServicio.clientAnswer.orderStatus),
                new SqlParameter("@p_effectiveBrand", rptaServicio.clientAnswer.transactions[0].transactionDetails.cardDetails.effectiveBrand),
                new SqlParameter("@p_externalTransactionId", rptaServicio.clientAnswer.transactions[0].transactionDetails.externalTransactionId),
                new SqlParameter("@p_Recibido", user.formToken),
                new SqlParameter("@p_Usuario", user.user),
                new SqlParameter("@p_ordenPago", rptaServicio.clientAnswer.orderDetails.orderId),
                new SqlParameter("@P_AUTOID", user.autoId)
            };
            try
            {
                _connectionBase.ExecuteByStoredProcedure("[sp_Izipay_INS]", parameter, ConnectionBase.enuTypeDataBase.sqlCon);
            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(3).GetFileLineNumber();
                Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
                throw ex;   // return rpta;
            }
            Log.save(this, "TERMINA METODO Pago_Save PASE PARAMETROS A PROC dbo.sp_Izipay_INS");
            return rpta;
        }

        public async Task<int> Orden_Pago(OrdenPago request)
        {
            // Grabar el resultado
            Log.save(this, "EMPIEZA METODO Orden_Pago PASE PARAMETROS A PROC dbo.sp_OrdenPago_INS");
            List<SqlParameter> parameters = new List<SqlParameter> {
                //new SqlParameter("@p_idOrdenPago", request.idOrdenPago),
                //new SqlParameter("@p_idProducto", request.idProducto),
                //new SqlParameter("@p_Estado", request.estado),
                new SqlParameter("@p_Usuario", request.User),
                new SqlParameter("@p_placa", request.Placa),
                new SqlParameter("@p_prima", request.Prima),
                new SqlParameter("@p_nroDoc", request.NroDocumento),
                new SqlParameter("@p_jsonAuto", request.JsonAuto),
                new SqlParameter("@NumOrden", SqlDbType.Int) {Direction = ParameterDirection.ReturnValue}
            };

            try
            {
                var Ret = _connectionBase.ExecuteByStoredProcedure("[sp_OrdenPago_INS]", parameters, ConnectionBase.enuTypeDataBase.sqlCon);
            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(3).GetFileLineNumber();
                Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
                throw ex;
            }
            request.idOrdenPago = (int)parameters[5].Value;
            Log.save(this, "TERMINA METODO Orden_Pago PASE PARAMETROS A PROC dbo.sp_OrdenPago_INS");
            return (int)parameters[5].Value;
        }

        public async Task<int> Validar_Pago(string Orden, decimal Monto)
        {
            int response = 0;
            Log.save(this, "EMPIEZA METODO Orden_Pago PASE PARAMETROS A PROC dbo.sp_OrdenPago_VAL");
            List<SqlParameter> parameters = new List<SqlParameter> {
                new SqlParameter("@p_OrdenPago", Orden),
                new SqlParameter("@p_Prima", Monto),
                new SqlParameter("@p_Existe", SqlDbType.Int) {Direction = ParameterDirection.Output}
            };

            try
            {
                SqlDataReader dr = (SqlDataReader)_connectionBase.ExecuteByStoredProcedure("sp_OrdenPago_VAL", parameters, ConnectionBase.enuTypeDataBase.sqlCon);
                response = (int)parameters[2].Value;
            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(3).GetFileLineNumber();
                Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
                throw ex;
            }
            Log.save(this, "TERMINA METODO Orden_Pago PASE PARAMETROS A PROC dbo.sp_OrdenPago_VAL");
            return response;
        }

        public Task<IEnumerable<OrdenPago>> GetOrden(OrdenPagoParam request)
        {
            List<OrdenPago> response = new List<OrdenPago>();
            List<SqlParameter> parameters = new List<SqlParameter> {
                new SqlParameter("@p_idOrdenPago", request.idOrdenPago),
                new SqlParameter("@p_placa", request.placa),
                new SqlParameter("@p_estado", request.estado),
                new SqlParameter("@p_NroDoc", request.nroDoc),
                new SqlParameter("@p_FchIni", request.fechaIni),
                new SqlParameter("@p_FchFin", request.fechaFin),
                new SqlParameter("@p_OrdenPago", request.ordenPago)
            };

            try
            {
                using (SqlDataReader dr = (SqlDataReader)_connectionBase.ExecuteByStoredProcedure("dbo.sp_OrdenPago_SEL", parameters, ConnectionBase.enuTypeDataBase.sqlCon))
                {
                    response = dr.ReadRowsList<OrdenPago>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Task.FromResult<IEnumerable<OrdenPago>>(response);
        }

        public async Task<ConsultaMasivaResponse> InsertaProcesoConsultaPlacasMasivo(ConsultaMasivaRequest consultaMasivaRequest)
        {
            ConsultaMasivaResponse response = new ConsultaMasivaResponse();
            Log.save(this, "EMPIEZA METODO InsertaProcesoConsultaPlacasMasivo PASE PARAMETROS A PROC dbo.sp_Ins_ProcesoConsultaPlacasAPESEG");
            List<SqlParameter> parameters = new List<SqlParameter> {
                new SqlParameter("@p_UsuarioRegistro", consultaMasivaRequest.Usuario),
                new SqlParameter("@p_UsuarioModifica", consultaMasivaRequest.Usuario),
                new SqlParameter("@p_ArchivoNombre", consultaMasivaRequest.ArchivoNombre),
                new SqlParameter("@p_Estado", "PE"),
                new SqlParameter("@p_InsertedId", SqlDbType.Int) {Direction = ParameterDirection.Output}
            };

            try
            {
                SqlDataReader dr = (SqlDataReader)_connectionBase.ExecuteByStoredProcedure("sp_Ins_ProcesoConsultaPlacasAPESEG", parameters, ConnectionBase.enuTypeDataBase.sqlCon);
                response.idProceso = (int)parameters[4].Value;


            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(3).GetFileLineNumber();
                Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
                throw ex;
            }
            Log.save(this, "TERMINA METODO InsertaProcesoConsultaPlacasMasivo PASE PARAMETROS A PROC dbo.sp_Ins_ProcesoConsultaPlacasAPESEG");
            return response;
        }

        public async Task<ConsultaMasivaResponse> InsertaPlacasMasivo(ConsultaMasivaRequest consultaMasivaRequest, DataTable PlacasLista)
        {
            ConsultaMasivaResponse response = new ConsultaMasivaResponse();
            Log.save(this, "EMPIEZA METODO InsertaPlacasMasivo PASE PARAMETROS A PROC dbo.sp_upd_ProcesoConsultaPlacasAPESEG");
            List<SqlParameter> parameters = new List<SqlParameter> {
            new SqlParameter("@p_IdProceso", SqlDbType.Int) { Value = consultaMasivaRequest.idProceso  },
            new SqlParameter("@p_Estado", SqlDbType.VarChar, 10) { Value = "PR" },
            new SqlParameter("@p_CantidadTotal", SqlDbType.Int) { Value = consultaMasivaRequest.CantidadTotal  },
            new SqlParameter("@p_Mensaje", SqlDbType.VarChar, 100) { Value = "" },
            new SqlParameter("@p_Usuario", SqlDbType.VarChar, 100) { Value = consultaMasivaRequest.Usuario  },
            new SqlParameter("@p_Result", SqlDbType.VarChar,1000) { Direction = ParameterDirection.Output }
             };

            var sConexion = "";//this.appSettings.Value.ConnectionString;

            sConexion = _connectionBase.ConnectionGetString(ConnectionBase.enuTypeDataBase.sqlCon);


            using (SqlConnection connection = new SqlConnection(sConexion))
            {
                connection.Open();

                // Iniciar la transacción
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {

                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
                    {
                        // Especificar el nombre de la tabla de destino
                        bulkCopy.DestinationTableName = "dbo.ApesegConsultaPlacaMasivoDetalle";

                        // Mapear las columnas de la DataTable con las columnas de la tabla de destino
                        bulkCopy.ColumnMappings.Add("Id", "Id");
                        bulkCopy.ColumnMappings.Add("IdProceso", "IdProceso");
                        bulkCopy.ColumnMappings.Add("FechaRegistro", "FechaRegistro");
                        bulkCopy.ColumnMappings.Add("FechaModificado", "FechaModificado");
                        bulkCopy.ColumnMappings.Add("UsuarioRegistro", "UsuarioRegistro");
                        bulkCopy.ColumnMappings.Add("UsuarioModifica", "UsuarioModifica");
                        bulkCopy.ColumnMappings.Add("NombreCompania", "NombreCompania");
                        bulkCopy.ColumnMappings.Add("FechaInicio", "FechaInicio");
                        bulkCopy.ColumnMappings.Add("FechaFinVigencia", "FechaFinVigencia");
                        bulkCopy.ColumnMappings.Add("FechaFin", "FechaFin");
                        bulkCopy.ColumnMappings.Add("FechaInicioVigencia", "FechaInicioVigencia");
                        bulkCopy.ColumnMappings.Add("FechaInicioD", "FechaInicioD");
                        bulkCopy.ColumnMappings.Add("FechaFinD", "FechaFinD");
                        bulkCopy.ColumnMappings.Add("Placa", "Placa");
                        bulkCopy.ColumnMappings.Add("Certificado", "Certificado");
                        bulkCopy.ColumnMappings.Add("NombreUsoVehiculo", "NombreUsoVehiculo");
                        bulkCopy.ColumnMappings.Add("NombreClaseVehiculo", "NombreClaseVehiculo");
                        bulkCopy.ColumnMappings.Add("Estado", "Estado");
                        bulkCopy.ColumnMappings.Add("CodigoSBSAseguradora", "CodigoSBSAseguradora");
                        bulkCopy.ColumnMappings.Add("CodigoUnicoPoliza", "CodigoUnicoPoliza");
                        bulkCopy.ColumnMappings.Add("EstaAnulado", "EstaAnulado");
                        bulkCopy.ColumnMappings.Add("Orden", "Orden");

                        // Agregar más mapeos de columnas según sea necesario

                        // Configurar opciones adicionales de SqlBulkCopy si es necesario
                        // bulkCopy.BatchSize = 100;
                        // bulkCopy.BulkCopyTimeout = 60;

                        // Ejecutar la inserción masiva
                        bulkCopy.WriteToServer(PlacasLista);
                    }

                    using (SqlCommand command = new SqlCommand("dbo.sp_upd_ProcesoConsultaPlacasAPESEG", connection, transaction))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(parameters.ToArray());

                        // Ejecutar el procedimiento almacenado
                        await command.ExecuteNonQueryAsync();
                    }

                    // Confirmar la transacción
                    transaction.Commit();

                    response.Mensaje = (string)parameters[3].Value;
                }
                catch (Exception ex)
                {
                    // Ocurrió un error, revertir la transacción
                    transaction.Rollback();
                    int line = (new StackTrace(ex, true)).GetFrame(3).GetFileLineNumber();
                    Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
                    throw ex;
                }
            }

            Log.save(this, "TERMINA METODO InsertaPlacasMasivo PASE PARAMETROS A PROC dbo.sp_upd_ProcesoConsultaPlacasAPESEG");
            return response;
        }

        public async Task<List<ApesegConsultaPlacaMasivo>> ConsultarProcesosMasivos()
        {
            List<ApesegConsultaPlacaMasivo> response = new List<ApesegConsultaPlacaMasivo>();

            Log.save(this, "EMPIEZA METODO ConsultarProcesosMasivos PASE PARAMETROS A PROC dbo.sp_ConsultarProcesosMasivosAPESEG_SEL");

            try
            {
                using (SqlDataReader dr = (SqlDataReader)_connectionBase.ExecuteByStoredProcedure("dbo.sp_ConsultarProcesosMasivosAPESEG_SEL", null, ConnectionBase.enuTypeDataBase.sqlCon))
                {
                    response = dr.ReadRowsList<ApesegConsultaPlacaMasivo>();
                }

            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(3).GetFileLineNumber();
                Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
                throw ex;
            }
            Log.save(this, "TERMINA METODO ConsultarProcesosMasivos PASE PARAMETROS A PROC dbo.sp_ConsultarProcesosMasivosAPESEG_SEL");
            return response;

        }

        public async Task<List<ApesegConsultaPlacaMasivoDetalle>> ConsultarProcesoDetalle(ConsultaMasivaRequest request)
        {
            List<ApesegConsultaPlacaMasivoDetalle> response = new List<ApesegConsultaPlacaMasivoDetalle>();

            Log.save(this, "EMPIEZA METODO ConsultarProcesoDetalle PASE PARAMETROS A PROC dbo.sp_ConsultarProcesosMasivoDetalleAPESEG_SEL");

            try
            {
                List<SqlParameter> parameters = new List<SqlParameter> {
                new SqlParameter("@p_IdProceso", SqlDbType.Int) { Value = request.idProceso },
                new SqlParameter("@p_Placa", SqlDbType.VarChar, 10) { Value = request.Placa },

             };
                using (SqlDataReader dr = (SqlDataReader)_connectionBase.ExecuteByStoredProcedure("dbo.sp_ConsultarProcesosMasivoDetalleAPESEG_SEL", parameters, ConnectionBase.enuTypeDataBase.sqlCon))
                {
                    response = dr.ReadRowsList<ApesegConsultaPlacaMasivoDetalle>();
                }

            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(3).GetFileLineNumber();
                Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
                throw ex;
            }
            Log.save(this, "TERMINA METODO ConsultarProcesoDetalle PASE PARAMETROS A PROC dbo.sp_ConsultarProcesosMasivoDetalleAPESEG_SEL");
            return response;

        }


        public async Task<ConfigAPESEG> ObtieneConfigAPESEG()
        {
            ConfigAPESEG response = new ConfigAPESEG();
            Log.save(this, "EMPIEZA METODO ObtieneConfigAPESEG PASE PARAMETROS A PROC dbo.fnc_TablaConfiguracion_valor");

            try
            {

                response.BatchSize = int.Parse((string)_connectionBase.ExecuteScalarSqlFunction("dbo.fnc_TablaConfiguracion_valor", new string[] { "APESEG.BatchSize", "0" }));
                response.DelayInSeconds = int.Parse((string)_connectionBase.ExecuteScalarSqlFunction("dbo.fnc_TablaConfiguracion_valor", new string[] { "APESEG.DelayInSeconds", "0" }));

            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(3).GetFileLineNumber();
                Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
                throw ex;
            }
            Log.save(this, "TERMINA METODO ObtieneConfigAPESEG PASE PARAMETROS A PROC dbo.fnc_TablaConfiguracion_valor");
            return response;
        }

        public async Task<ConsultaMasivaResponse> ActualizaErrorPlacasMasivo(ConsultaMasivaRequest consultaMasivaRequest)
        {
            ConsultaMasivaResponse response = new ConsultaMasivaResponse();
            Log.save(this, "EMPIEZA METODO ActualizaErrorPlacasMasivo PASE PARAMETROS A PROC dbo.sp_upd_ProcesoConsultaPlacasAPESEG");
            List<SqlParameter> parameters = new List<SqlParameter> {
            new SqlParameter("@p_IdProceso", SqlDbType.Int) { Value = consultaMasivaRequest.idProceso  },
            new SqlParameter("@p_Estado", SqlDbType.VarChar, 10) { Value = "ER" },
            new SqlParameter("@p_CantidadTotal", SqlDbType.Int) { Value = consultaMasivaRequest.CantidadTotal  },
            new SqlParameter("@p_Mensaje", SqlDbType.VarChar, 1000) { Value = consultaMasivaRequest.Mensaje  },
            new SqlParameter("@p_Usuario", SqlDbType.VarChar, 1000) { Value = consultaMasivaRequest.Usuario   },
            new SqlParameter("@p_Result", SqlDbType.VarChar,1000) { Direction = ParameterDirection.Output }
             };

            var sConexion = "";//this.appSettings.Value.ConnectionString;

            sConexion = _connectionBase.ConnectionGetString(ConnectionBase.enuTypeDataBase.sqlCon);

            using (SqlConnection connection = new SqlConnection(sConexion))
            {
                connection.Open();

                // Iniciar la transacción
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {

                    using (SqlCommand command = new SqlCommand("dbo.sp_upd_ProcesoConsultaPlacasAPESEG", connection, transaction))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(parameters.ToArray());

                        // Ejecutar el procedimiento almacenado
                        await command.ExecuteNonQueryAsync();
                    }

                    // Confirmar la transacción
                    transaction.Commit();

                    response.Mensaje = (string)parameters[5].Value;
                }
                catch (Exception ex)
                {
                    // Ocurrió un error, revertir la transacción
                    transaction.Rollback();
                    int line = (new StackTrace(ex, true)).GetFrame(3).GetFileLineNumber();
                    Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
                    throw ex;
                }
            }

            Log.save(this, "TERMINA METODO ActualizaErrorPlacasMasivo PASE PARAMETROS A PROC dbo.sp_upd_ProcesoConsultaPlacasAPESEG");
            return response;
        }


        public async Task<ConsultaMasivaResponse> ActualizaPlacasMasivo(ConsultaMasivaRequest consultaMasivaRequest, ApesegConsultaPlacaMasivoDetalle Placa)
        {

            ConsultaMasivaResponse response = new ConsultaMasivaResponse();
            Log.save(this, "EMPIEZA METODO ActualizaPlacasMasivo PASE PARAMETROS A PROC dbo.sp_upd_ProcesoPlacaAPESEG");

            var sConexion = "";//this.appSettings.Value.ConnectionString;

            sConexion = _connectionBase.ConnectionGetString(ConnectionBase.enuTypeDataBase.sqlCon);

            using (SqlConnection connection = new SqlConnection(sConexion))
            {
                connection.Open();

                // Iniciar la transacción
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {

                    List<SqlParameter> parametersPlaca = new List<SqlParameter>
                    {
                    new SqlParameter("@P_IdProceso", SqlDbType.Int) { Value = Placa.IdProceso },
                    new SqlParameter("@P_Placa", SqlDbType.VarChar, 255) { Value = Placa.Placa },
                    new SqlParameter("@P_UsuarioModifica", SqlDbType.VarChar, 10) { Value = consultaMasivaRequest.Usuario },
                    new SqlParameter("@P_NombreCompania", SqlDbType.VarChar, 255) { Value = Placa.NombreCompania ?? "" },
                    new SqlParameter("@P_FechaInicio", SqlDbType.VarChar, 255) { Value = Placa.FechaInicio ?? "" },
                    new SqlParameter("@P_FechaFinVigencia", SqlDbType.DateTime) { Value = Placa.FechaFinVigencia != null ? (object)Placa.FechaFinVigencia : DBNull.Value },
                    new SqlParameter("@P_FechaFin", SqlDbType.VarChar, 255) { Value = Placa.FechaFin ?? "" },
                    new SqlParameter("@P_FechaInicioVigencia", SqlDbType.DateTime) { Value = Placa.FechaInicioVigencia != null ? (object)Placa.FechaInicioVigencia : DBNull.Value },
                    new SqlParameter("@P_FechaInicioD", SqlDbType.DateTime) { Value = Placa.FechaInicioD != null ? (object)Placa.FechaInicioD : DBNull.Value },
                    new SqlParameter("@P_FechaFinD", SqlDbType.DateTime) { Value = Placa.FechaFinD != null ? (object)Placa.FechaFinD : DBNull.Value},
                    new SqlParameter("@P_Certificado", SqlDbType.VarChar, 255) { Value = Placa.Certificado ?? "" },
                    new SqlParameter("@P_NombreUsoVehiculo", SqlDbType.VarChar, 255) { Value = Placa.NombreUsoVehiculo ?? "" },
                    new SqlParameter("@P_NombreClaseVehiculo", SqlDbType.VarChar, 255) { Value = Placa.NombreClaseVehiculo ?? "" },
                    new SqlParameter("@P_Estado", SqlDbType.VarChar, 255) { Value = Placa.Estado ?? ""},
                    new SqlParameter("@P_CodigoSBSAseguradora", SqlDbType.VarChar, 255) { Value = Placa.CodigoSBSAseguradora ?? "" },
                    new SqlParameter("@P_CodigoUnicoPoliza", SqlDbType.VarChar, 255) { Value = Placa.CodigoUnicoPoliza ?? "" },
                    new SqlParameter("@P_EstaAnulado", SqlDbType.VarChar, 255) { Value = Placa.EstaAnulado ?? "" },
                    new SqlParameter("@P_Revisado", SqlDbType.Char, 2) { Value = Placa.Revisado },
                    new SqlParameter("@P_Orden", SqlDbType.Int) { Value = Placa.Orden },
                    new SqlParameter("@p_Result", SqlDbType.VarChar,1000) { Direction = ParameterDirection.Output }
                    };

                    using (SqlCommand command = new SqlCommand("dbo.sp_upd_ProcesoPlacaAPESEG", connection, transaction))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(parametersPlaca.ToArray());

                        // Ejecutar el procedimiento almacenado
                        await command.ExecuteNonQueryAsync();
                    }


                    List<SqlParameter> parameters = new List<SqlParameter> {
                    new SqlParameter("@p_IdProceso", SqlDbType.Int) { Value = consultaMasivaRequest.idProceso  },
                    new SqlParameter("@p_Estado", SqlDbType.VarChar, 10) { Value = consultaMasivaRequest.Estado },
                    new SqlParameter("@p_CantidadTotal", SqlDbType.Int) { Value = consultaMasivaRequest.CantidadTotal  },
                    new SqlParameter("@p_Mensaje", SqlDbType.VarChar, 100) { Value = consultaMasivaRequest.Mensaje  },
                    new SqlParameter("@p_Usuario", SqlDbType.VarChar, 100) { Value = consultaMasivaRequest.Usuario },
                    new SqlParameter("@p_Result", SqlDbType.VarChar,1000) { Direction = ParameterDirection.Output }
                    };
                    using (SqlCommand command = new SqlCommand("dbo.sp_upd_ProcesoConsultaPlacasAPESEG", connection, transaction))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(parameters.ToArray());

                        // Ejecutar el procedimiento almacenado
                        await command.ExecuteNonQueryAsync();
                    }

                    // Confirmar la transacción
                    transaction.Commit();

                    response.Mensaje = (string)parameters[5].Value;
                }
                catch (Exception ex)
                {
                    // Ocurrió un error, revertir la transacción
                    transaction.Rollback();
                    int line = (new StackTrace(ex, true)).GetFrame(3).GetFileLineNumber();
                    Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
                    throw ex;
                }
            }

            Log.save(this, "TERMINA METODO ActualizaPlacasMasivo PASE PARAMETROS A PROC dbo.sp_upd_ProcesoPlacaAPESEG");
            return response;

        }


        public async Task<ConsultaMasivaResponse> ActualizaProcesoMasivo(ConsultaMasivaRequest request)
        {
            ConsultaMasivaResponse response = new ConsultaMasivaResponse();

            Log.save(this, "EMPIEZA METODO ActualizaProcesoMasivo PASE PARAMETROS A PROC dbo.sp_upd_ProcesoConsultaPlacasAPESEG");

            List<SqlParameter> parameters = new List<SqlParameter> {
            new SqlParameter("@p_IdProceso", SqlDbType.Int) { Value = request.idProceso  },
            new SqlParameter("@p_Estado", SqlDbType.VarChar, 10) { Value =request.Estado},
            new SqlParameter("@p_CantidadTotal", SqlDbType.Int) { Value = request.CantidadTotal  },
            new SqlParameter("@p_Mensaje", SqlDbType.VarChar, 100) { Value = request.Mensaje  },
            new SqlParameter("@p_Usuario", SqlDbType.VarChar, 250) { Value = request.Usuario  },
            new SqlParameter("@p_Result", SqlDbType.VarChar,1000) { Direction = ParameterDirection.Output }
            };

            try
            {
                SqlDataReader dr = (SqlDataReader)_connectionBase.ExecuteByStoredProcedure("dbo.sp_upd_ProcesoConsultaPlacasAPESEG", parameters, ConnectionBase.enuTypeDataBase.sqlCon);
                response.Mensaje = (string)parameters[5].Value;
            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(3).GetFileLineNumber();
                Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
                throw ex;
            }
            Log.save(this, "TERMINA METODO ActualizaProcesoMasivo PASE PARAMETROS A PROC dbo.sp_upd_ProcesoConsultaPlacasAPESEG");
            return response;

        }

        public async Task<EnvioCarritoResponse> InsertaEnvioCarrito(EnvioCarritoRequest request)
        {
            var response = new EnvioCarritoResponse();
            var storeProc = "soat.sp_Carrito_Envio_INS";
            var metodo = "InsertaEnvioCarrito";

            Log.save(this, $"EMPIEZA METODO {metodo} PASE PARAMETROS A PROC {storeProc}");

            List<SqlParameter> parameters = new List<SqlParameter> {
            new SqlParameter("@p_Intentos", SqlDbType.Int) { Value = request.Toque  },
            new SqlParameter("@p_Result", SqlDbType.VarChar,1000) { Direction = ParameterDirection.Output }
            };

            try
            {

                using (SqlDataReader dr = (SqlDataReader)_connectionBase.ExecuteByStoredProcedure(storeProc, parameters, ConnectionBase.enuTypeDataBase.sqlCon))
                {
                    var placas = new List<string>();

                    while (dr.Read())
                    {
                        placas.Add(dr["Placa"].ToString());
                    }

                    response.Placas = placas;

                }

                response.Mensaje = parameters[1].Value.ToString();

            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(3).GetFileLineNumber();
                Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
                throw ex;
            }
            Log.save(this, $"TERMINA METODO {metodo} PASE PARAMETROS A PROC {storeProc}");
            return response;

        }

        public async Task<EnvioCarritoResponse> ActualizaEnvioCarrito(ActualizaCarritoRequest request)
        {
            var response = new EnvioCarritoResponse();
            var storeProc = "soat.sp_Carrito_Envio_UPD";
            var metodo = "ActualizaEnvioCarrito";

            Log.save(this, $"EMPIEZA METODO {metodo} PASE PARAMETROS A PROC {storeProc}");

            try
            {
                foreach (var placaRequest in request.Placas)
                {
                    // Parámetros para el procedimiento almacenado
                    List<SqlParameter> parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@P_Placa", SqlDbType.NVarChar, 20) { Value = placaRequest.Placa },
                        new SqlParameter("@P_Comprado", SqlDbType.Bit) { Value = placaRequest.Comprado },
                        new SqlParameter("@P_FechaInicioApeseg", SqlDbType.NVarChar,20) { Value = placaRequest.FechaInicioApeseg ?? string .Empty },
                        new SqlParameter("@P_FechaFinApeseg", SqlDbType.NVarChar,20) { Value = placaRequest.FechaFinApeseg ?? string .Empty},
                        new SqlParameter("@P_CompaniaApeseg", SqlDbType.NVarChar,100) { Value = placaRequest.NombreCompania ?? string .Empty},
                        new SqlParameter("@P_UsuarioModificacion", SqlDbType.NVarChar, 30) { Value = "SYS" }
                    };

                    // Ejecutar el procedimiento almacenado
                    SqlDataReader dr = (SqlDataReader)_connectionBase.ExecuteByStoredProcedure(storeProc, parameters, ConnectionBase.enuTypeDataBase.sqlCon);

                    var registrosActualizados = string.Empty;
                    while (dr.Read())
                    {
                        registrosActualizados = dr["RegistrosActualizados"].ToString();
                    }

                    response.Placas.Add(placaRequest.Placa);

                    Log.save(this, $"Placa {placaRequest.Placa} actualizada. Registros afectados: {registrosActualizados}");
                }

                response.Mensaje = "Actualización completada correctamente.";
            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(3).GetFileLineNumber();
                Log.save(this, $"ERROR EN LINEA: {line} / {ex.Message}");
                throw;
            }

            Log.save(this, $"TERMINA METODO {metodo} PASE PARAMETROS A PROC {storeProc}");
            return response;
        }


    }
}
