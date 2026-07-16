using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using VidaCamara.CrossCuting.Utilities;
using VidaCamara.CrossCuting.Utilities.Configuration;
using VidaCamara.Domain.Services.Entidades.Configurar;
using VidaCamara.Domain.Services.Repositorios.Configurar;
using VidaCamara.Infrastructure.Connection;


namespace VidaCamara.Infrastructure.Data.Configurar
{
    public class RolRepository : IRolRepository
    {
        private readonly IOptions<AppSettings> appSettings;
        private readonly IConnectionBase _connectionBase;

        public RolRepository(IOptions<AppSettings> appSettings,   IConnectionBase ConnectionBase)
        {
            this.appSettings = appSettings;
            _connectionBase = ConnectionBase;
        }


        public Task<IEnumerable<Rol>> GetRol(int param)
        {
            IEnumerable<Rol> result = null;
            List<Rol> lRol = new List<Rol>();
            Log.save(this, "EMPIEZA METODO GetRol PASE PARAMETROS A PROC dbo.sp_ModuloPerfil_SEL");
            List<SqlParameter> parameters = new List<SqlParameter> { new SqlParameter("@p_idPerfil", param) };
            try
            {
                using (SqlDataReader dr = (SqlDataReader)_connectionBase.ExecuteByStoredProcedure("sp_ModuloPerfil_SEL",
                                      parameters, ConnectionBase.enuTypeDataBase.sqlCon))
                {
                    while (dr.Read())
                    {
                        var nGrupo = dr.GetOrdinal("grupo");
                        var oRol = new Rol
                        {
                            estado = dr.GetBoolean(dr.GetOrdinal("Estado")) ? 1 : 0,
                            idModulo = dr.GetInt32(dr.GetOrdinal("idModulo")),
                            descripcion = dr.GetString(dr.GetOrdinal("descripcion")),
                            url = dr.GetString(dr.GetOrdinal("url")),
                            idRol = dr.GetInt32(dr.GetOrdinal("idRol")),
                            grupo = dr.IsDBNull(nGrupo) ? " " : dr.GetString(nGrupo),
                            acceso = dr.GetString(dr.GetOrdinal("Acceso")),
                            usuarioCreacion = dr.GetString(dr.GetOrdinal("UsuarioCreacion")),
                            fechaRegistro = dr.GetString(dr.GetOrdinal("FechaRegistro")),
                        };
                        lRol.Add(oRol);
                    };
                    result = lRol as IEnumerable<Rol>;
                }
            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(3).GetFileLineNumber();
                Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
            }

            Log.save(this, "TERMINA METODO GetRol PASE PARAMETROS A PROC dbo.sp_ModuloPerfil_SEL");
            return Task.FromResult<IEnumerable<Rol>>(result);
        }

        public Task<Rol> InsRol(Rol param)
        {
            Log.save(this, "EMPIEZA METODO InsRol PASE PARAMETROS A PROC dbo.sp_ModuloPerfil_INS");
            List<SqlParameter> parameters = new List<SqlParameter> {
               new SqlParameter("@p_idRol", param.idRol),
               new SqlParameter("@p_idPerfil", param.idPerfil),
               new SqlParameter("@p_idModulo", param.idModulo),
               new SqlParameter("@p_Acceso", param.acceso),
               new SqlParameter("@p_Estado", param.estado),
               new SqlParameter("@p_Usuario", param.user)
            };
            try {
                _connectionBase.ExecuteByStoredProcedure("sp_ModuloPerfil_INS", parameters, ConnectionBase.enuTypeDataBase.sqlCon);
                param.result = 1;
            }
            catch (Exception ex)  {
                int line = (new StackTrace(ex, true)).GetFrame(3).GetFileLineNumber();
                Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
                param.result = 0;
                throw ex;
            }

            Log.save(this, "TERMINA METODO InsRol PASE PARAMETROS A PROC dbo.sp_ModuloPerfil_INS");
            return Task.FromResult<Rol>(param);
        }

 
 
    }
}

