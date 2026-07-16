using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using static VidaCamara.Infrastructure.Connection.ConnectionBase;

namespace VidaCamara.Infrastructure.Connection
{
    public interface IConnectionBase
    {
        DbParameterCollection ParamsCollectionResult { get; set; }
        DbConnection ConnectionGet(ConnectionBase.enuTypeDataBase typeDataBase = ConnectionBase.enuTypeDataBase.sqlCon);
        DbDataReader ExecuteByStoredProcedure(string nameStore, IEnumerable<DbParameter> parameters = null,
            ConnectionBase.enuTypeDataBase typeDataBase = ConnectionBase.enuTypeDataBase.sqlCon,
            ConnectionBase.enuTypeExecute typeExecute = ConnectionBase.enuTypeExecute.ExecuteReader
            );
        DbDataReader ExecuteByStoredProcedure_TRX(string nameStore,
               IEnumerable<DbParameter> parameters = null,
               DbConnection connection = null, DbTransaction trx = null,
               ConnectionBase.enuTypeDataBase typeDataBase = ConnectionBase.enuTypeDataBase.sqlCon,
               ConnectionBase.enuTypeExecute typeExecute = ConnectionBase.enuTypeExecute.ExecuteReader);
        object ExecuteByStoredFunction(string nameFunction, IEnumerable<DbParameter> parameters = null,
           ConnectionBase.enuTypeDataBase typeDataBase = ConnectionBase.enuTypeDataBase.sqlCon,
           ConnectionBase.enuTypeExecute typeExecute = ConnectionBase.enuTypeExecute.ExecuteReader
           );

        object ExecuteScalarSqlFunction(string functionName, string[] FunctionParameters);
        Task<DbDataReader> ExecuteByStoredProcedureAsync(string nameStore,
             IEnumerable<DbParameter> parameters = null,
             enuTypeDataBase typeDataBase = enuTypeDataBase.sqlCon,
             enuTypeExecute typeExecute = enuTypeExecute.ExecuteReader
             );
        string ConnectionGetString(enuTypeDataBase typeDataBase = enuTypeDataBase.sqlCon);
    }
}
