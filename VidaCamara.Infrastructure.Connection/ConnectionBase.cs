using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using VidaCamara.CrossCuting.Utilities.Configuration;
using VVSClave;

namespace VidaCamara.Infrastructure.Connection
{
    public class ConnectionBase : IConnectionBase
    {
        private string strConexionOracle = null;
        private string strConexionRI = null;
        private string strConexionSepelio = null;
        private string strConexionCRM = null;
        SqlConnection DataConnectionSQL = new SqlConnection();
        SqlConnection DataConnectionSQLRI = new SqlConnection();
        SqlConnection DataConnectionSQLSepelio = new SqlConnection();
        SqlConnection DataConnectionSQLCRM = new SqlConnection();
        

        private readonly AppSettings _appSettings;

        public enum enuTypeDataBase
        {
            sqlCon,
            sqlConRI,
            sqlConSepelio,
            sqlConCRM

        }

        public enum enuTypeExecute
        {
            ExecuteNonQuery,
            ExecuteReader
        }

        public DbParameterCollection ParamsCollectionResult { get; set; }

        public ConnectionBase(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;

            strConexionOracle = Clave.Decrypt(_appSettings.ConnectionString);
            DataConnectionSQL.ConnectionString = strConexionOracle;

            strConexionRI = Clave.Decrypt(_appSettings.ConnectionStringRI);
            DataConnectionSQLRI.ConnectionString = strConexionRI;

            strConexionCRM = Clave.Decrypt(_appSettings.ConnectionStringCRM);
            DataConnectionSQLCRM.ConnectionString = strConexionCRM;

            strConexionSepelio = Clave.Decrypt(_appSettings.ConnectionStringSepelio);
            DataConnectionSQLSepelio.ConnectionString = strConexionSepelio;
        }


        public DbConnection ConnectionGet(enuTypeDataBase typeDataBase = enuTypeDataBase.sqlCon)
        {
            DbConnection DataConnection = null;
            switch (typeDataBase)
            {
                case enuTypeDataBase.sqlCon:                
                    DataConnection = DataConnectionSQL;
                    break;
                case enuTypeDataBase.sqlConCRM:
                    DataConnection = DataConnectionSQLCRM;
                    break;
                case enuTypeDataBase.sqlConRI:
                    DataConnection = DataConnectionSQLRI;
                    break;
                case enuTypeDataBase.sqlConSepelio:
                    DataConnection = DataConnectionSQLSepelio;
                    break;
                default:
                    break;
            }
            return DataConnection;
        }

        public string ConnectionGetString(enuTypeDataBase typeDataBase = enuTypeDataBase.sqlCon)
        {
            string DataConnection = "";
            switch (typeDataBase)
            {
                case enuTypeDataBase.sqlCon:
                    DataConnection = strConexionOracle;
                    break;
                case enuTypeDataBase.sqlConCRM:
                    DataConnection = strConexionCRM;
                    break;
                case enuTypeDataBase.sqlConSepelio:
                    DataConnection = strConexionSepelio;
                    break;
                default:
                    break;
            }
            return DataConnection;
        }

        public DbDataReader ExecuteByStoredProcedure(string nameStore,
                IEnumerable<DbParameter> parameters = null,
                enuTypeDataBase typeDataBase = enuTypeDataBase.sqlCon,
                enuTypeExecute typeExecute = enuTypeExecute.ExecuteReader
                )
        {
            DbConnection DataConnection = ConnectionGet(typeDataBase);
            DbCommand cmdCommand = DataConnection.CreateCommand();
            try
            {

                cmdCommand.CommandText = nameStore;
                cmdCommand.CommandType = CommandType.StoredProcedure;

                if (parameters != null)
                {
                    foreach (DbParameter parameter in parameters)
                    {
                        cmdCommand.Parameters.Add(parameter);
                    }
                }

                if (DataConnection.State == ConnectionState.Open)
                {
                    DataConnection.Close();
                }

                DataConnection.Open();
                DbDataReader myReader;
                if (typeExecute == enuTypeExecute.ExecuteReader)
                {
                    myReader = cmdCommand.ExecuteReader(CommandBehavior.CloseConnection);
                }
                else
                {
                    myReader = null;
                    cmdCommand.ExecuteNonQuery();
                    if (DataConnection.State == ConnectionState.Open)
                    {
                        DataConnection.Close();
                    }

                }
                //myReader = cmdCommand.ExecuteReader(CommandBehavior.CloseConnection);

                return myReader;
            }
            catch (Exception)
            {
                DataConnection.Close();
                DataConnection.Dispose();
                cmdCommand.Dispose();
                throw;
            }

        }

        public async Task<DbDataReader> ExecuteByStoredProcedureAsync(string nameStore,
             IEnumerable<DbParameter> parameters = null,
             enuTypeDataBase typeDataBase = enuTypeDataBase.sqlCon,
             enuTypeExecute typeExecute = enuTypeExecute.ExecuteReader
             )
        {
            DbConnection DataConnection = ConnectionGet(typeDataBase);
            DbCommand cmdCommand = DataConnection.CreateCommand();
            try
            {

                cmdCommand.CommandText = nameStore;
                cmdCommand.CommandType = CommandType.StoredProcedure;

                if (parameters != null)
                {
                    foreach (DbParameter parameter in parameters)
                    {
                        cmdCommand.Parameters.Add(parameter);
                    }
                }

                if (DataConnection.State == ConnectionState.Open)
                {
                    DataConnection.Close();
                }

                DataConnection.Open();
                DbDataReader myReader;
                if (typeExecute == enuTypeExecute.ExecuteReader)
                {
                    myReader = await cmdCommand.ExecuteReaderAsync(CommandBehavior.CloseConnection);
                }
                else
                {
                    myReader = null;
                    await cmdCommand.ExecuteNonQueryAsync();
                    if (DataConnection.State == ConnectionState.Open)
                    {
                        DataConnection.Close();
                    }

                }
                //myReader = cmdCommand.ExecuteReader(CommandBehavior.CloseConnection);

                return myReader;
            }
            catch (Exception)
            {
                DataConnection.Close();
                DataConnection.Dispose();
                cmdCommand.Dispose();
                throw;
            }

        }

        public DbDataReader ExecuteByStoredProcedure_TRX(string nameStore,
                IEnumerable<DbParameter> parameters = null,
                DbConnection connection = null, DbTransaction trx = null,
                enuTypeDataBase typeDataBase = enuTypeDataBase.sqlCon,
                enuTypeExecute typeExecute = enuTypeExecute.ExecuteReader)
        {
            DbCommand cmdCommand = connection.CreateCommand();
            cmdCommand.Transaction = trx;
            cmdCommand.Connection = connection;

            cmdCommand.CommandText = nameStore;
            cmdCommand.CommandType = CommandType.StoredProcedure;

            if (parameters != null)
            {
                foreach (DbParameter parameter in parameters)
                {
                    cmdCommand.Parameters.Add(parameter);
                }
            }

            DbDataReader myReader;

            cmdCommand.ExecuteNonQuery();
            ParamsCollectionResult = cmdCommand.Parameters;
            myReader = null;

            return myReader;
        }

        public object ExecuteByStoredFunction(string nameFunction,
                IEnumerable<DbParameter> parameters = null,
                enuTypeDataBase typeDataBase = enuTypeDataBase.sqlCon,
                enuTypeExecute typeExecute = enuTypeExecute.ExecuteReader
                )
        {

            DbConnection DataConnection = ConnectionGet(typeDataBase);
            DbCommand cmdCommand = DataConnection.CreateCommand();
            cmdCommand.CommandText = nameFunction;
            cmdCommand.CommandType = CommandType.Text;


            if (parameters != null)
            {
                foreach (DbParameter parameter in parameters)
                {
                    cmdCommand.Parameters.Add(parameter);
                }
            }

            DataConnection.Open();

            var respuesta = cmdCommand.ExecuteScalar();

            return respuesta;
        }

        public object ExecuteScalarSqlFunction(string functionName, string[] FunctionParameters)
        {
            using (SqlConnection connection = new SqlConnection(strConexionOracle))
            {
                string query = "select " + functionName + "(";
                int index = 0;
                object result;
                foreach (string item in FunctionParameters)//{0},{0}
                {
                    query += String.Format("'{0}'", item);
                    query += ++index >= FunctionParameters.Length ? String.Format(")", item) : ",";
                }

                if (connection.State != ConnectionState.Open)
                { connection.Open(); }

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    result = cmd.ExecuteScalar();
                    connection.Close();
                    return result;
                }
            }
        }
    }
}
