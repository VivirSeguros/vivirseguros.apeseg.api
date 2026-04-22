using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using VVSClave;

namespace VivirSeguros.CRM.Infrastructure.Persistences.Contexts
{
    public class ConnectionFactory : IConnectionFactory
    {
        private readonly IConfiguration _configuration;

        public ConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection GetConnection
        {
            get
            {
                var sqlConnection = new SqlConnection();
                try
                {
                    
                    if (sqlConnection == null) return null;

                    var cadena = _configuration.GetConnectionString("ConnectionString");
                    sqlConnection.ConnectionString = Clave.Decrypt(cadena);
                    sqlConnection.Open();
                   
                }
                catch (System.Exception ex)
                {
                    throw;
                }
                return sqlConnection;
            }
        }
        
        public IDbConnection GetConnection1
        {
            get
            {
                var sqlConnection = new SqlConnection();
                try
                {

                    if (sqlConnection == null) return null;

                    sqlConnection.ConnectionString = _configuration.GetConnectionString("ConnectionString1");
                    sqlConnection.Open();

                }
                catch (System.Exception ex)
                {
                    throw;
                }
                return sqlConnection;
            }
        }
        
        public IDbConnection GetConnection2
        {
            get
            {
                var sqlConnection = new SqlConnection();
                try
                {

                    if (sqlConnection == null) return null;

                    sqlConnection.ConnectionString = _configuration.GetConnectionString("ConnectionString2");
                    sqlConnection.Open();

                }
                catch (System.Exception ex)
                {
                    throw;
                }
                return sqlConnection;
            }
        }
        public IDbConnection GetConnection3
        {
            get
            {
                var sqlConnection = new SqlConnection();
                try
                {

                    if (sqlConnection == null) return null;

                    sqlConnection.ConnectionString = _configuration.GetConnectionString("ConnectionString3");
                    sqlConnection.Open();

                }
                catch (System.Exception ex)
                {
                    throw;
                }
                return sqlConnection;
            }
        }

        public IDbConnection GetConnection4
        {
            get
            {
                var sqlConnection = new SqlConnection();
                try
                {

                    if (sqlConnection == null) return null;

                    sqlConnection.ConnectionString = _configuration.GetConnectionString("ConnectionString4");
                    sqlConnection.Open();

                }
                catch (System.Exception ex)
                {
                    throw;
                }
                return sqlConnection;
            }
        }


        public IDbConnection GetConnection5
        {
            get
            {
                var sqlConnection = new SqlConnection();
                try
                {

                    if (sqlConnection == null) return null;

                    var cadena = _configuration.GetConnectionString("ConnectionString5");
                    sqlConnection.ConnectionString = Clave.Decrypt(cadena);
                    sqlConnection.Open();


                }
                catch (System.Exception ex)
                {
                    throw;
                }
                return sqlConnection;
            }
        }




    }
}
