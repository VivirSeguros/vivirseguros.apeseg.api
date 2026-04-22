using System.Threading.Tasks;
using VivirSeguros.CRM.Domain.Entities.Billing;

namespace VivirSeguros.CRM.Infrastructure.Persistences.Interfaces
{
    public interface IBillRepository
    {
        Task<BillFactBase> GetAsync(string skey);
    }
}
