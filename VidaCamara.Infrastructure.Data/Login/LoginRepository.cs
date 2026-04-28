using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Options;
using VidaCamara.Domain.Services.LoginModule;
using VidaCamara.CrossCuting.Utilities.Configuration;
using VidaCamara.Infrastructure.Connection;
using System.Threading.Tasks;
using System.Data.SqlClient;
using VidaCamara.Domain.Services.Entidades.Login;
using VidaCamara.CrossCuting.Utilities;
using System.Diagnostics;
using System.Data;

namespace VidaCamara.Infrastructure.Data.LoginModule
{
    public class LoginRepository : ILoginRepository
    {
        private readonly IOptions<AppSettings> appSettings;
        private readonly IConnectionBase _connectionBase;

        public LoginRepository(IOptions<AppSettings> appSettings,
                               IConnectionBase ConnectionBase)
        {
            this.appSettings = appSettings;
            _connectionBase = ConnectionBase;
        }
        public Task<IEnumerable<Modulo>> GetMenu(int param)
        {
            IEnumerable<Modulo> entities = null;
            List<Modulo> lModulo = new List<Modulo>();
            Log.save(this, "EMPIEZA METODO GetMenu PASE PARAMETROS A PROC dbo.sp_ModuloPerfil_SEL");
            List<SqlParameter> parameters = new List<SqlParameter> {
                new SqlParameter("@p_idPerfil", param)
            };
            try
            {
                using (SqlDataReader dr =
            (SqlDataReader)_connectionBase.ExecuteByStoredProcedure("sp_ModuloPerfil_SEL",
             parameters, ConnectionBase.enuTypeDataBase.sqlCon))
                {
                    while (dr.Read())
                    {
                        var objLogin = new Modulo
                        {
                            idModulo = dr.GetInt32(dr.GetOrdinal("idModulo")),
                            Descripcion = dr.GetString(dr.GetOrdinal("Descripcion")),
                            Nivel = dr.GetInt32(dr.GetOrdinal("Nivel")),
                            Acceso = dr.GetString(dr.GetOrdinal("Acceso")),
                            Estado = dr.GetBoolean(dr.GetOrdinal("Estado")),
                            Url = dr["url"] == DBNull.Value ? "" : dr.GetString(dr.GetOrdinal("Url"))
                        };
                        if (objLogin.Estado) lModulo.Add(objLogin);
                    }
                    entities = lModulo as IEnumerable<Modulo>;
                }
            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(3).GetFileLineNumber();
                Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
            }

            Log.save(this, "TERMINA METODO GetMenu PASE PARAMETROS A PROC dbo.sp_ModuloPerfil_SEL");
            return Task.FromResult<IEnumerable<Modulo>>(entities);
        }

        public Task<Login> GetLogin(LoginParam param)
        {
            Login result = null;
            Log.save(this, "EMPIEZA METODO GetLogin PASE PARAMETROS A PROC dbo.sp_Login_SEL");
            List<SqlParameter> parameters = new List<SqlParameter> {
                new SqlParameter("@p_login", param.username),
                new SqlParameter("@p_clave", param.password)
            };

            try
            {
                using (SqlDataReader dr =
            (SqlDataReader)_connectionBase.ExecuteByStoredProcedure("sp_Login_SEL", parameters, ConnectionBase.enuTypeDataBase.sqlCon))
                {
                    while (dr.Read())
                    {
                        result = new Login
                        {
                            idUsuario = dr.GetInt32(dr.GetOrdinal("idUsuario")),
                            login = dr.GetString(dr.GetOrdinal("Username")),
                            nombreUsuario = dr.GetString(dr.GetOrdinal("Usuario")),
                            idPerfil = dr.GetInt32(dr.GetOrdinal("idTipoPerfil")),
                            desPerfil = dr.GetString(dr.GetOrdinal("Descripcion")),
                            idPtoVenta = dr.GetInt32(dr.GetOrdinal("idPuntoVenta")),
                            nombrePtoVenta = dr.GetString(dr.GetOrdinal("PtoVenta")),
                            idCanal = dr.GetInt32(dr.GetOrdinal("idCanal")),
                            nombreCanal = dr.GetString(dr.GetOrdinal("Canal")),
                            ActiveDirectory = dr.GetBoolean(dr.GetOrdinal("ActiveDirectory")),
                        };
                    };
                }
            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(3).GetFileLineNumber();
                Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
            }

            Log.save(this, "TERMINA METODO GetLogin PASE PARAMETROS A PROC dbo.sp_Login_SEL");
            return Task.FromResult<Login>(result);
        }

        public Task<IEnumerable<Combo>> GetCombo(ComboParam param)
        {
            IEnumerable<Combo> entities = null;
            List<Combo> lCombo = new List<Combo>();
            List<SqlParameter> parameters = new List<SqlParameter> {
                new SqlParameter("@p_Tabla", param.tabla),
                new SqlParameter("@p_Campo1", param.campo1),
                new SqlParameter("@p_Campo2", param.campo2)
            };
            try
            {
                using (SqlDataReader dr =
            (SqlDataReader)_connectionBase.ExecuteByStoredProcedure("sp_Combo_SEL",
             parameters, ConnectionBase.enuTypeDataBase.sqlCon))
                {
                    while (dr.Read())
                    {
                        var objCombo = new Combo
                        {
                            id = dr.GetInt32(dr.GetOrdinal("id")),
                            descripcion = dr.GetString(dr.GetOrdinal("descripcion"))
                        };
                        lCombo.Add(objCombo);
                    };
                    entities = lCombo as IEnumerable<Combo>;
                }
            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(3).GetFileLineNumber();
                Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
            }
            return Task.FromResult<IEnumerable<Combo>>(entities);
        }
        public Task<bool> CheckIfAD(string username)
        {
            bool isAd = false;


            List<SqlParameter> parameters = new List<SqlParameter>
            {
            };
            SqlParameter sqlParam1 = new SqlParameter("@p_username", username);
            SqlParameter sqlParam2 = new SqlParameter("@p_result", DbType.Boolean);
            sqlParam2.Direction = ParameterDirection.Output;
            parameters.Add(sqlParam1);
            parameters.Add(sqlParam2);
            try
            {
                using (SqlDataReader dr =
            (SqlDataReader)_connectionBase.ExecuteByStoredProcedure("sp_CheckIfAD_SEL",
             parameters, ConnectionBase.enuTypeDataBase.sqlCon, typeExecute: ConnectionBase.enuTypeExecute.ExecuteNonQuery))
                {
                    isAd = Convert.ToBoolean(sqlParam2.Value);
                }

            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(3).GetFileLineNumber();
                Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
            }
            return Task.FromResult<bool>(isAd);
        }
    }
}
