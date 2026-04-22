using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VivirSeguros.CRM.Infrastructure.Persistences.Contexts;
using VivirSeguros.CRM.Infrastructure.Persistences.Interfaces;

namespace VivirSeguros.CRM.Infrastructure.Persistences.Repositories
{
    public class HelperRepository : IHelperRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public HelperRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public Task<string> GetValorTablaConfig(string skey)
        {
            string response = "";

            using (var connection = _connectionFactory.GetConnection)
            {
                var query = "soat.fnc_TablaConfiguracion_valor";

                var result = connection.QueryFirstOrDefault("select soat.fnc_TablaConfiguracion_valor(@P_SKEY)", new { P_SKEY = skey}, commandType: CommandType.Text);

                foreach (var item in result)
                {
                    response = item.Value;
                }
                return Task.FromResult(response);
            }
        }
    }
}
