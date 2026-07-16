using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using VidaCamara.CrossCuting.Utilities.Configuration;
using VidaCamara.Infrastructure.Connection;
using System.Threading.Tasks;
using System.Data.SqlClient;
using VidaCamara.Domain.Services.Entidades.Configurar;
using VidaCamara.Domain.Services.Repositorios.Configurar;
using VidaCamara.CrossCuting.Utilities;
using System.Diagnostics;

namespace VidaCamara.Infrastructure.Data.Configurar
{
    public class TipoPerfilRepository : ITipoPerfilRepository
    {
        private readonly IOptions<AppSettings> appSettings;
        private readonly IConnectionBase _connectionBase;

        public TipoPerfilRepository(IOptions<AppSettings> appSettings,
                                    IConnectionBase ConnectionBase)
        {
            this.appSettings = appSettings;
            _connectionBase = ConnectionBase;
        }


        public Task<IEnumerable<TipoPerfil>> GetTipoPerfil(int param, int prd)
        {
            IEnumerable<TipoPerfil> result = null;
            List<TipoPerfil> lTipoPerfil = new List<TipoPerfil>();
            Log.save(this, "EMPIEZA METODO GetTipoPerfil PASE PARAMETROS A PROC dbo.sp_TipoPerfil_SEL");
            List<SqlParameter> parameters = new List<SqlParameter> {
               new SqlParameter("@p_idTipoPerfil", param),
               new SqlParameter("@p_idProducto", prd)
            };
            try
            {
                using (SqlDataReader dr = (SqlDataReader)_connectionBase.ExecuteByStoredProcedure("sp_TipoPerfil_SEL",
                                       parameters, ConnectionBase.enuTypeDataBase.sqlCon))
                {
                    while (dr.Read())
                    {
                        var oTipoPerfil = new TipoPerfil
                        {
                            idTipoPerfil = dr.GetInt32(dr.GetOrdinal("idTipoPerfil")),
                            idProducto = dr.IsDBNull(1) ? 0 : dr.GetInt32(1),
                            descripcion = dr.GetString(dr.GetOrdinal("Descripcion")),
                            descripcionCorta = dr.GetString(dr.GetOrdinal("DescripcionCorta")),
                            usuarioCreacion = dr.GetString(dr.GetOrdinal("UsuarioCreacion")),
                            fechaRegistro = dr.GetString(dr.GetOrdinal("FechaRegistro")),
                            estado = dr.GetBoolean(dr.GetOrdinal("Estado")) ? 1 : 0
                        };
                        lTipoPerfil.Add(oTipoPerfil);
                    };
                    result = lTipoPerfil as IEnumerable<TipoPerfil>;
                }
                Log.save(this, "TERMINA METODO GetTipoPerfil PASE PARAMETROS A PROC dbo.sp_TipoPerfil_SEL");
            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(3).GetFileLineNumber();
                Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
            }
            
            return Task.FromResult<IEnumerable<TipoPerfil>>(result);
        }

        public Task<TipoPerfil> InsTipoPerfil(TipoPerfil param)
        {
            Log.save(this, "EMPIEZA METODO InsTipoPerfil PASE PARAMETROS A PROC dbo.sp_TipoPerfil_INS");
            List<SqlParameter> parameters = new List<SqlParameter> {
               new SqlParameter("@p_idTipoPerfil", param.idTipoPerfil),
               new SqlParameter("@p_idProducto", param.idProducto),
               new SqlParameter("@p_Descripcion", param.descripcion),
               new SqlParameter("@p_DescripcionCorta", param.descripcionCorta),
               new SqlParameter("@p_Estado", param.estado),
               new SqlParameter("@p_Usuario", param.user)
            };
            try {
                _connectionBase.ExecuteByStoredProcedure("sp_TipoPerfil_INS", parameters, ConnectionBase.enuTypeDataBase.sqlCon);
                param.result = 1;
                Log.save(this, "TERMINA METODO InsTipoPerfil PASE PARAMETROS A PROC dbo.sp_TipoPerfil_INS");
            }
            catch (Exception ex) {
                int line = (new StackTrace(ex, true)).GetFrame(3).GetFileLineNumber();
                Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
                throw ex;
                param.result = 0;
            }

            return Task.FromResult<TipoPerfil>(param);
        }
    }
}
