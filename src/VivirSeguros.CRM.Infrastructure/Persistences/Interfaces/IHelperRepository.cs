using System.Threading.Tasks;

namespace VivirSeguros.CRM.Infrastructure.Persistences.Interfaces
{
    public interface IHelperRepository
    {
        Task<string> GetValorTablaConfig(string skey);
    }
}
