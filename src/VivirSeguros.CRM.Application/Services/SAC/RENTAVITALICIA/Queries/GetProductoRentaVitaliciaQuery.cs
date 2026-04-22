using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VivirSeguros.CRM.Application.Commons;

namespace VivirSeguros.CRM.Application.Services.SAC.RENTAVITALICIA.Queries
{
    public class GetProductoRentaVitaliciaQuery : IRequest<Response<IEnumerable<GetProductoRentaVitaliciaViewModel>>>
    {
        public string NroPoliza { get; set; }
        public string NroDocumento { get; set; }
    }
}
