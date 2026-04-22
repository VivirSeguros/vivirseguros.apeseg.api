using System.Data;

namespace VivirSeguros.CRM.Infrastructure.Persistences.Contexts
{
    public interface IConnectionFactory
    {
        IDbConnection GetConnection { get; }
        IDbConnection GetConnection1 { get; }
        IDbConnection GetConnection2 { get; }
        IDbConnection GetConnection3 { get; }
        IDbConnection GetConnection4 { get; }
        IDbConnection GetConnection5 { get; }


    }
}
