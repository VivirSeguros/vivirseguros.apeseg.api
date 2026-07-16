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

        public Task Apeseg_Insertar(ApesegLog param)
        {
            List<SqlParameter> parameters = new List<SqlParameter> {
                new SqlParameter("@p_TipoEnvio",param.Tipo),
                new SqlParameter("@p_NumCertificado", param.Nro ?? ""),
                new SqlParameter("@p_Digito", param.Digito),
                new SqlParameter("@p_Error", param.Err ?? ""),
                new SqlParameter("@p_Enviado", param.Envia ?? ""),
                new SqlParameter("@p_Recibido",param.Recibe ?? ""),
                new SqlParameter("@p_Usuario", param.User ?? "SYS"),
                new SqlParameter("@p_Proveedor", param.Proveedor ?? ""),
                new SqlParameter("@p_Canal", param.Canal ?? ""),
                new SqlParameter("@p_PuntoVenta", param.PuntoVenta ?? ""),
                new SqlParameter("@p_IpCliente", param.IpCliente ?? ""),
                new SqlParameter("@p_TramaEnvio", param.TramaEnvio ?? ""),
                new SqlParameter("@p_TramaRespuesta", param.TramaRespuesta ?? "")
            };
            try
            {
                _connectionBase.ExecuteByStoredProcedure("[sp_Apeseg_INS]", parameters, ConnectionBase.enuTypeDataBase.sqlCon);
            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(0)?.GetFileLineNumber() ?? 0;
                Log.save(this, "ERROR GRABAR LOG EN LINEA: " + line + " / " + ex.Message);
                throw;
            }
            return Task.CompletedTask;
        }
        public Task<IEnumerable<ApesegErrorCatalogo>> ApesegErrorCatalogo_Listar()
        {
            IEnumerable<ApesegErrorCatalogo> result = null;
            List<ApesegErrorCatalogo> lErrores = new List<ApesegErrorCatalogo>();

            Log.save(this, "EMPIEZA METODO ApesegErrorCatalogo_Listar PROC sp_ApesegErrorCatalogo_SEL");

            try
            {
                using (SqlDataReader dr = (SqlDataReader)_connectionBase.ExecuteByStoredProcedure("[sp_ApesegErrorCatalogo_SEL]", null, ConnectionBase.enuTypeDataBase.sqlCon))
                {
                    while (dr.Read())
                    {
                        var oError = new ApesegErrorCatalogo
                        {
                            Codigo = dr.GetString(dr.GetOrdinal("Codigo")),
                            Descripcion = dr.GetString(dr.GetOrdinal("Descripcion")),
                        };
                        lErrores.Add(oError);
                    }
                    result = lErrores as IEnumerable<ApesegErrorCatalogo>;
                }
            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(0)?.GetFileLineNumber() ?? 0;
                Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
            }

            Log.save(this, "TERMINA METODO ApesegErrorCatalogo_Listar");
            return Task.FromResult<IEnumerable<ApesegErrorCatalogo>>(result);
        }
        public Task<IEnumerable<string>> Apeseg_Validar(string tipo, dynamic req)
        {
            List<SqlParameter> p = new List<SqlParameter>();
            IEnumerable<string> result = null;
            List<string> lErrores = new List<string>();

            Log.save(this, "EMPIEZA METODO Apeseg_Validar");

            object GetVal(string propName)
            {
                try
                {
                    var value = req.GetType().GetProperty(propName)?.GetValue(req, null);
                    return value ?? DBNull.Value;
                }
                catch
                {
                    return DBNull.Value;
                }
            }

            p.Add(new SqlParameter("@p_Tipo", tipo));
            p.Add(new SqlParameter("@p_CodigoAseguradora", GetVal("CodigoAseguradora")));
            p.Add(new SqlParameter("@p_PolizaCertificado", GetVal("PolizaCertificado")));
            p.Add(new SqlParameter("@p_DigitoVerificador", GetVal("DigitoVerificador")));

            p.Add(new SqlParameter("@p_FechaInicioVigencia", GetVal("FechaInicioVigencia")));
            p.Add(new SqlParameter("@p_FechaFinVigencia", GetVal("FechaFinVigencia")));
            p.Add(new SqlParameter("@p_FechaControlPolicial", GetVal("FechaControlPolicial")));

            p.Add(new SqlParameter("@p_FechaIngreso", GetVal("FechaIngreso")));
            p.Add(new SqlParameter("@p_FechaActualizacion", GetVal("FechaActualizacion"))); 
            p.Add(new SqlParameter("@p_FechaAnulacion", GetVal("FechaAnulacion")));

            p.Add(new SqlParameter("@p_CodigoTipoPersona", GetVal("CodigoTipoPersona")));
            p.Add(new SqlParameter("@p_NombreContratante", GetVal("NombreContratante")));
            p.Add(new SqlParameter("@p_CodigoTipoDocumento", GetVal("CodigoTipoDocumento")));
            p.Add(new SqlParameter("@p_NumeroDocumento", GetVal("NumeroDocumento")));
            p.Add(new SqlParameter("@p_TelefonoContacto", GetVal("TelefonoContacto")));
            p.Add(new SqlParameter("@p_CorreoContacto", GetVal("CorreoContacto")));

            p.Add(new SqlParameter("@p_PlacaVehiculo", GetVal("PlacaVehiculo")));
            p.Add(new SqlParameter("@p_CodigoUsoVehiculo", GetVal("CodigoUsoVehiculo")));
            p.Add(new SqlParameter("@p_CodigoClaseVehiculo", GetVal("CodigoClaseVehiculo")));
            p.Add(new SqlParameter("@p_PaisPlaca", GetVal("PaisPlaca")));
            p.Add(new SqlParameter("@p_Marca", GetVal("Marca")));
            p.Add(new SqlParameter("@p_ModeloVehiculo", GetVal("ModeloVehiculo")));
            p.Add(new SqlParameter("@p_NumeroAsientos", GetVal("NumeroAsientos")));
            p.Add(new SqlParameter("@p_NumeroSerieMotor", GetVal("NumeroSerieMotor")));
            p.Add(new SqlParameter("@p_NumeroSerieChasis", GetVal("NumeroSerieChasis")));
            p.Add(new SqlParameter("@p_CodigoUbigeo", GetVal("CodigoUbigeo")));

            p.Add(new SqlParameter("@p_TipoCertificado", GetVal("TipoCertificado")));
            p.Add(new SqlParameter("@p_CodigoTipoAnulacion", GetVal("CodigoTipoAnulacion")));

            p.Add(new SqlParameter("@p_UsuarioRegistro", GetVal("UsuarioRegistro")));
            p.Add(new SqlParameter("@p_Proveedor", GetVal("Proveedor")));
            p.Add(new SqlParameter("@p_Canal", GetVal("Canal")));
            p.Add(new SqlParameter("@p_PuntoVenta", GetVal("PuntoVenta")));
            p.Add(new SqlParameter("@p_IpCliente", GetVal("IpCliente")));


            try
            {
                using (SqlDataReader dr = (SqlDataReader)_connectionBase.ExecuteByStoredProcedure("[sp_Apeseg_VAL]", p, ConnectionBase.enuTypeDataBase.sqlCon))
                {
                    while (dr.Read())
                    {
                        lErrores.Add(dr.GetString(dr.GetOrdinal("MensajeCompleto")));
                    }
                    result = lErrores as IEnumerable<string>;
                }
            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(0)?.GetFileLineNumber() ?? 0;
                Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
            }

            Log.save(this, "TERMINA METODO Apeseg_Validar");
            return Task.FromResult<IEnumerable<string>>(result);
        }
    }
}