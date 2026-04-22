using System.Collections.Generic;
using System.Threading.Tasks;
using VivirSeguros.CRM.Domain.Common;
using VivirSeguros.CRM.Domain.Entities;

namespace VivirSeguros.CRM.Infrastructure.Persistences.Interfaces
{
    public interface IJobRepository
    {
        Task<bool> InsertAsync(Job job);
        Task<BaseTransaction> UpdateAsync(Job job);
        Task<IEnumerable<Job>> GetByJobTypeAsync(int jobTypeId);
    }
}
