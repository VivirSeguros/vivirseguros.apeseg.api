using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VivirSeguros.CRM.Application.Commons;

namespace VivirSeguros.CRM.Application.Services.SAC.RENTAMAX.Queries
{
    public class GetAseguradoRentamaxQuery : IRequest<Response<IEnumerable<GetAseguradoRentamaxViewModel>>>
    {
        public string NroPoliza { get; set; }
        public string NroDocumento { get; set; }
    }
}
