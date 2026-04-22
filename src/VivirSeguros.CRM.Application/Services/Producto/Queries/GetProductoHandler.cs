using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VivirSeguros.CRM.Application.Commons;
using VivirSeguros.CRM.Domain.Enums;
using VivirSeguros.CRM.Domain.Extensions;
using VivirSeguros.CRM.Infrastructure.Persistences.Interfaces;

namespace VivirSeguros.CRM.Application.Services.Producto.Queries
{
    public class GetProductoHandler : IRequestHandler<GetProductoQuery, Response<IEnumerable<GetProductoViewModel>>>
    {
        private readonly IAVirtualRepository _avirtualRepository;
        private readonly IMapper _mapper;

        public GetProductoHandler(IAVirtualRepository avirtualRepository, IMapper mapper)
        {
            _avirtualRepository = avirtualRepository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<GetProductoViewModel>>> Handle(GetProductoQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<IEnumerable<GetProductoViewModel>>();
            try
            {
                var info = await _avirtualRepository.GetProducto(request.NroDoc);
                if (info is not null)
                {
                    response.Data = _mapper.Map<List<GetProductoViewModel>>(info);
                    response.IsSuccess = true;
                    response.Message = ResponseMessage.SucessfulQuery.GetStringValue();
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
