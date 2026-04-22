using VivirSeguros.CRM.Domain.Entities;
using System.Threading.Tasks;
using VivirSeguros.CRM.Domain.Common;

namespace VivirSeguros.CRM.Infrastructure.Persistences.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account> GetAsync(string userName);
        Task<BaseTransaction> InsertAsync(Account account);
    }
}