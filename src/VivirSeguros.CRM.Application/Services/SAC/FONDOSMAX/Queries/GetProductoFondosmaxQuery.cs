using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VivirSeguros.CRM.Application.Commons;

namespace VivirSeguros.CRM.Application.Services.SAC.FONDOSMAX.Queries
{
    public class GetProductoFondosmaxQuery : IRequest<Response<IEnumerable<GetProductoFondosmaxViewModel>>>
    {
        public string NroPoliza { get; set; }
        public string NroDocumento { get; set; }
    }
}
