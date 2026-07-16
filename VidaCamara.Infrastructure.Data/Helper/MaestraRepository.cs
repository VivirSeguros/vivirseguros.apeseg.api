using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using VidaCamara.CrossCuting.Utilities;
using VidaCamara.CrossCuting.Utilities.Configuration;
using VidaCamara.Domain.Services.Entidades.Helper;
using VidaCamara.Domain.Services.Repositorios.Helper;
using VidaCamara.Infrastructure.Connection;

namespace VidaCamara.Infrastructure.Data.Helper
{
    public class MaestraRepository : IMaestraRepository
    {
        private readonly IOptions<AppSettings> appSettings;
        private readonly IConnectionBase _connectionBase;

        public MaestraRepository(IOptions<AppSettings> appSettings,
                               IConnectionBase ConnectionBase)
        {
            this.appSettings = appSettings;
            this._connectionBase = ConnectionBase;
        }

        public Task<IEnumerable<Maestra>> GetTipoCanal_SEL()
        {
            List<Maestra> response = new List<Maestra>();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                using (SqlDataReader dr = (SqlDataReader)_connectionBase.ExecuteByStoredProcedure("dbo.sp_TipoCanal_SEL", parameters, ConnectionBase.enuTypeDataBase.sqlCon))
                {
                    response = dr.ReadRowsList<Maestra>();
                }
            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(3).GetFileLineNumber();
                Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
                throw ex;
            }

            return Task.FromResult<IEnumerable<Maestra>>(response);
        }

        public Task<IEnumerable<Maestra>> TipoDocumento_sel(MaestraParam idMaestra)
        {
            List<Maestra> response = new List<Maestra>();
            List<SqlParameter> parameters = new List<SqlParameter>();

            try
            {
                parameters.Add(new SqlParameter("@P_IDMAESTRA", value: idMaestra.idMaestra));
                using (SqlDataReader dr = (SqlDataReader)_connectionBase.ExecuteByStoredProcedure("dbo.sp_TipoDocumento_SEL", parameters, ConnectionBase.enuTypeDataBase.sqlCon))
                {
                    response = dr.ReadRowsList<Maestra>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Task.FromResult<IEnumerable<Maestra>>(response);
        }

        public Task<IEnumerable<Maestra>> TipoPersona_sel()
        {
            List<Maestra> response = new List<Maestra>();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                using (SqlDataReader dr = (SqlDataReader)_connectionBase.ExecuteByStoredProcedure("dbo.sp_TipoPersona_SEL", parameters, ConnectionBase.enuTypeDataBase.sqlCon))
                {
                    response = dr.ReadRowsList<Maestra>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Task.FromResult<IEnumerable<Maestra>>(response);
        }
    }
}
