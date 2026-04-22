using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VivirSeguros.CRM.Application.Commons;

namespace VivirSeguros.CRM.Application.Services.Cuenta.Queries
{
    public class CuentaQuery : IRequest<Response<IEnumerable<CuentaViewModel>>>
    {
        public string NroDoc { get; set; }
    }
}
