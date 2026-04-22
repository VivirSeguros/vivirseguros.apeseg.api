using VivirSeguros.CRM.Domain.Entities;
using VivirSeguros.CRM.Infrastructure.Persistences.Contexts;
using VivirSeguros.CRM.Infrastructure.Persistences.Interfaces;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using VivirSeguros.CRM.Domain.Common;
using VVSClave;

namespace VivirSeguros.CRM.Infrastructure.Persistences.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public AccountRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Account> GetAsync(string userName)
        {
            using (var connection = _connectionFactory.GetConnection)
            {
                var query = "dbo.sp_Login_SEL";
                var parameters = new DynamicParameters();
                parameters.Add("p_login", userName);

                var account = await connection.QuerySingleOrDefaultAsync<Account>(query, param: parameters, commandType: CommandType.StoredProcedure);
                account.Secret = Clave.Decrypt(account.Secret);
                return account;
            }
        }
        public async Task<BaseTransaction> InsertAsync(Account account)
        {
            BaseTransaction result = new BaseTransaction();
            using (var connection = _connectionFactory.GetConnection)
            {
                var query = "dbo.sp_Login_INS";
                var parameters = new DynamicParameters();
                parameters.Add("Username", account.Username);
                parameters.Add("Secret", account.Secret);
                parameters.Add("P_COD_ERROR", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("P_MENSAJE", dbType: DbType.String, direction: ParameterDirection.Output, size: 100);
                connection.Execute(query, param: parameters, commandType: CommandType.StoredProcedure);
                result.ErrorCode = parameters.Get<int?>("P_COD_ERROR");
                result.Message = parameters.Get<string>("P_MENSAJE");
                await connection.ExecuteAsync(query, param: parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
        }
    }
}
