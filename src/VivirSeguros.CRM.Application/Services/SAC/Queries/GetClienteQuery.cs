using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VivirSeguros.CRM.Application.Commons;

namespace VivirSeguros.CRM.Application.Services.SAC.Queries
{
    public class GetClienteQuery : IRequest<Response<IEnumerable<GetClienteViewModel>>>
    {
        public string NroPoliza { get; set; }
        public string NroDocumento { get; set; }
        public string Placa { get; set; }
        public string Nombre1 { get; set; }
        public string ApellidoPaterno { get; set; }

    }
}
