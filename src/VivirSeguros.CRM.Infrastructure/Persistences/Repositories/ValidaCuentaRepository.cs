
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VivirSeguros.CRM.Domain.Entities.Producto;
using VivirSeguros.CRM.Domain.Entities.ValidaCuenta;
using VivirSeguros.CRM.Infrastructure.Persistences.Contexts;
using VivirSeguros.CRM.Infrastructure.Persistences.Interfaces;

namespace VivirSeguros.CRM.Infrastructure.Persistences.Repositories
{
    public class ValidaCuentaRepository : IValidaCuentaRepository
    {
        private readonly IConnectionFactory _connectionFactory;
        public ValidaCuentaRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Cuenta>> ValidaCuenta(string nroDoc)
        {
            //nroDoc = "44837570";
            
            using (var connection = _connectionFactory.GetConnection)
            {
                var query = "sp_CRM_Titulares_SEL";
                var parameters = new DynamicParameters();
                parameters.Add("@@NRODOCUMENTO", nroDoc);  

                var response = await connection.QueryAsync<Cuenta>(query, param: parameters, commandType: CommandType.StoredProcedure);

                return response;
            }
        }
    }
}
