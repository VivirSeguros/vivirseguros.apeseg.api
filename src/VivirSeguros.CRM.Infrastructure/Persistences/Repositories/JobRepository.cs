using Dapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using VivirSeguros.CRM.Domain.Common;
using VivirSeguros.CRM.Domain.Entities;
using VivirSeguros.CRM.Infrastructure.Persistences.Contexts;
using VivirSeguros.CRM.Infrastructure.Persistences.Interfaces;

namespace VivirSeguros.CRM.Infrastructure.Persistences.Repositories
{
    public class JobRepository : IJobRepository
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly ILogger<JobRepository> _logger;

        public JobRepository(IConnectionFactory connectionFactory, ILogger<JobRepository> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<bool> InsertAsync(Job job)
        {
            using (var connection = _connectionFactory.GetConnection)
            {
                var query = "dbo.sp_Trabajo_INS";
                var parameters = new DynamicParameters();
                parameters.Add("P_IDPRODUCTO", job.ProductId);
                parameters.Add("P_IDTIPOTRABAJO", job.TypeJobId);
                parameters.Add("P_IDPAGO", job.PaymentId);
                //parameters.Add("P_SKEY", job.Skey);
                parameters.Add("P_ESTADO", job.State);
                parameters.Add("P_USUARIOCREACION", job.CreationUser);
                /*parameters.Add("P_COD_ERROR", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("P_MENSAJE", dbType: DbType.String, direction: ParameterDirection.Output);*/

                var result = await connection.ExecuteAsync(query, param: parameters, commandType: CommandType.StoredProcedure);
                return result > 0;
            }
        }

        public async Task<BaseTransaction> UpdateAsync(Job job)
        {
            BaseTransaction result = new BaseTransaction();
            using (var connection = _connectionFactory.GetConnection)
            {
                try
                {
                    var query = "dbo.sp_Trabajo_UPD";
                    var parameters = new DynamicParameters();
                    parameters.Add("P_IDPRODUCTO", job.ProductId);
                    parameters.Add("P_IDTIPOTRABAJO", job.TypeJobId);
                    parameters.Add("P_SKEY", job.Skey);
                    parameters.Add("P_REQUEST", job.DetailJob.Request);
                    parameters.Add("P_REQUEST1", job.DetailJob.Request1);
                    parameters.Add("P_RESPONSE", job.DetailJob.Response);
                    parameters.Add("P_RESPONSECODE", job.DetailJob.ResponseCode);
                    parameters.Add("P_ESTADO", job.State);
                    parameters.Add("P_USUARIOCREACION", job.CreationUser);
                    parameters.Add("P_COD_ERROR", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    parameters.Add("P_MENSAJE", dbType: DbType.String, direction: ParameterDirection.Output, size: 100);
                    connection.Execute(query, param: parameters, commandType: CommandType.StoredProcedure);
                    result.ErrorCode = parameters.Get<int>("P_COD_ERROR");
                    result.Message = parameters.Get<string>("P_MENSAJE");
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex.Message);
                    connection.Close();
                    throw ex;
                }  
            }
            return result;
        }


        public async Task<IEnumerable<Job>> GetByJobTypeAsync(int jobTypeId)
        {
            
            using (var connection = _connectionFactory.GetConnection)
            {
                var query = "dbo.sp_Trabajo_GET_IDTYPE";
                var parameters = new DynamicParameters();
                parameters.Add("P_IDTIPOTRABAJO", jobTypeId);

                var response = await connection.QueryAsync<Job>(query, param: parameters, commandType: CommandType.StoredProcedure);
                return response;
            }
        }
    }
}
