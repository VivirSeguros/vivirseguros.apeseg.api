using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VivirSeguros.CRM.Application.Commons;

namespace VivirSeguros.CRM.Application.Services.Producto.Queries
{
    public class GetProductoQuery : IRequest<Response<IEnumerable<GetProductoViewModel>>>
    {
        public string NroDoc { get; set; }
    }
}
