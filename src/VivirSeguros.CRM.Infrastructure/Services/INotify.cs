using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivirSeguros.CRM.Infrastructure.Services
{
    public interface INotify
    {
        Task Publish(string message);
    }
}
