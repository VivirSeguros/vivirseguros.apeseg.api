using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VivirSeguros.CRM.Domain.Entities.ValidaCuenta;

namespace VivirSeguros.CRM.Infrastructure.Persistences.Interfaces
{
    public interface IValidaCuentaRepository
    {
        Task<IEnumerable<Cuenta>> ValidaCuenta(string nroDoc);
    }
}
