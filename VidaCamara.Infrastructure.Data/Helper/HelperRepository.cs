using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using VidaCamara.CrossCuting.Utilities;
using VidaCamara.CrossCuting.Utilities.Configuration;
using VidaCamara.Domain.Services.Entidades;
using VidaCamara.Domain.Services.Entidades.Helper;
using VidaCamara.Domain.Services.Repositorios.Helper;
using VidaCamara.Infrastructure.Connection;

namespace VidaCamara.Infrastructure.Data.Helper
{
    public class HelperRepository : IHelperRepository
    {
        private readonly IOptions<AppSettings> appSettings;
        private readonly IConnectionBase _connectionBase;

        public HelperRepository(IOptions<AppSettings> appSettings,
                               IConnectionBase ConnectionBase)
        {
            this.appSettings = appSettings;
            _connectionBase = ConnectionBase;
        }

        public Task<IEnumerable<Combo>> GetCombo(int comboId, int ventaDigital, int filtro, int filtro1)
        {
            IEnumerable<Combo> result = null;
            List<Combo> response = new List<Combo>();
            List<SqlParameter> parameters = new List<SqlParameter>();

            try
            {
                parameters.Add(new SqlParameter("@p_idCombo", value: comboId));
                parameters.Add(new SqlParameter("@P_VENTADIGITAL", value: ventaDigital));
                parameters.Add(new SqlParameter("@P_FILTRO1", value: filtro));
                parameters.Add(new SqlParameter("@P_FILTRO2", value: filtro1));
                using (SqlDataReader dr = (SqlDataReader)_connectionBase.ExecuteByStoredProcedure("dbo.sp_Combo_SEL", parameters, ConnectionBase.enuTypeDataBase.sqlCon))
                {
                    foreach (var item in dr.ReadRowsList<Combo>())
                    {
                        response.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(3).GetFileLineNumber();
                Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
                throw ex;
            }
            /*List<SqlParameter> parameters = new List<SqlParameter> { new SqlParameter("@p_idCombo", comboId, ) };

            using (SqlDataReader dr = (SqlDataReader)_connectionBase.ExecuteByStoredProcedure("sp_Combo_SEL",
                                       parameters, ConnectionBase.enuTypeDataBase.sqlCon))
            {
                while (dr.Read()) {
                    var oCombo = new Combo {
                        id = dr.GetValue(dr.GetOrdinal("id")),
                        descripcion = dr.GetString(dr.GetOrdinal("descripcion")),
                        valor = dr.GetValue(dr.GetOrdinal("valor"))
                    };
                    lCombo.Add(oCombo);
                };
                result = lCombo as IEnumerable<Combo>;
            }*/
            return Task.FromResult<IEnumerable<Combo>>(response);
        }

        public Task<Correo> GetCorreoInfo(int autoId)
        {
            Correo response = new Correo();
            List<SqlParameter> parameters = new List<SqlParameter>();
            try
            {
                Log.save(this, "EMPIEZA METODO GetCorreoInfo PASE PARAMETROS A PROC dbo.sp_Correo_GET_info");
                parameters.Add(new SqlParameter("@P_IDAUTO", value: autoId));
          
                using (SqlDataReader dr = (SqlDataReader)_connectionBase.ExecuteByStoredProcedure("dbo.sp_Correo_GET_info", parameters, ConnectionBase.enuTypeDataBase.sqlCon))
                {
                    foreach (var item in dr.ReadRowsList<Correo>())
                    {
                        response = item;
                    }
                }
            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(3).GetFileLineNumber();
                Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
                throw ex;
            }
            Log.save(this, "TERMINA METODO GetCorreoInfo PASE PARAMETROS A PROC dbo.sp_Correo_GET_info");
            return Task.FromResult(response);
        }

        public Task<ResponseTrx> GuardarDocumentoLog(LogTransacParam request)
        {
            ResponseTrx response = new ResponseTrx();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string fecha = null;
            try
            {
                Log.save(this, "EMPIEZA METODO GuardarDocumentoLog PASE PARAMETROS A PROC dbo.sp_LogTransac_INS");
                parameters.Add(new SqlParameter("@P_IDAUTO", value: request.IdAuto));
                parameters.Add(new SqlParameter("@P_DESCRIPCION", value: request.Descripcion));
                parameters.Add(new SqlParameter("@P_ERROR", value: request.Error));
                parameters.Add(new SqlParameter("@P_URLDOCUMENTO", value: request.UrlDocumento));
                parameters.Add(new SqlParameter("@P_PASO", value: request.Paso));
                parameters.Add(new SqlParameter("@P_TIPOLOG", value: request.TipoLog));


                SqlDataReader dr = (SqlDataReader)_connectionBase.ExecuteByStoredProcedure("dbo.sp_LogTransac_INS", parameters, ConnectionBase.enuTypeDataBase.sqlCon);
            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(3).GetFileLineNumber();
                Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
                response.Codigo = 1;
                response.Mensaje = ex.Message;
                throw ex;
            }
            Log.save(this, "TERMINA METODO GuardarDocumentoLog PASE PARAMETROS A PROC dbo.sp_LogTransac_INS");
            return Task.FromResult(response);
        }

        public Task<string> GetValorTablaConfig(string skey)
        {
            string response = "";

            List<SqlParameter> parameters = new List<SqlParameter>();
            try
            {
                response =  _connectionBase.ExecuteScalarSqlFunction("soat.fnc_TablaConfiguracion_valor", new string[] { skey }).ToString();
                //parameters.Add(new SqlParameter("@P_IDPLAN", value: request.IdPlan));
                //fecha = (DateTime?) _connectionBase.ExecuteByStoredFunction("select soat.fnc_PlanHistoria_FinVig(@P_IDPLAN)", parameters, ConnectionBase.enuTypeDataBase.sqlCon);

            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(3).GetFileLineNumber();
                Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
                throw ex;
            }

            return Task.FromResult<string>(response);
        }


        public Task<string> GetUrlDocumentoSepelioCampofeAsync(int idCampofeTrama)
        {
            string response = string.Empty;

            try
            {
                // Nombre de la función (SIN SELECT)
                string functionName = "Sepelio.fnc_Campofe_GET_UrlDoc";

                // Parámetros como string[]
                string[] functionParameters = { idCampofeTrama.ToString() };

                var result = _connectionBase.ExecuteScalarSqlFunction(
                    functionName,
                    functionParameters
                );

                response = result?.ToString();
            }
            catch (Exception)
            {
                throw;
            }

            return Task.FromResult(response);
        }

        public Task<string> GetTableConfigAsync(string skey, int productId)
        {
            string response = string.Empty;

            try
            {
                // Nombre de la función (SIN SELECT)
                string functionName = "dbo.fnc_TablaConfiguracion_valor";

                // Parámetros como string[]
                string[] functionParameters = { skey.ToString(), productId.ToString() };

                var result = _connectionBase.ExecuteScalarSqlFunction(
                    functionName,
                    functionParameters
                );

                response = result?.ToString();
            }
            catch (Exception)
            {
                throw;
            }

            return Task.FromResult(response);
        }


    }
}
