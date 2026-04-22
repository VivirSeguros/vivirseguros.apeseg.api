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

namespace VivirSeguros.CRM.Application.Services.SAC.FONDOSMAX.Queries
{
    public class  GetProductoFondosmaxHandler : IRequestHandler<GetProductoFondosmaxQuery, Response<IEnumerable<GetProductoFondosmaxViewModel>>>
    {
        private readonly IAVirtualRepository _avirtualRepository;
        private readonly IMapper _mapper;

        public GetProductoFondosmaxHandler(IAVirtualRepository avirtualRepository, IMapper mapper)
        {
            _avirtualRepository = avirtualRepository;
            _mapper = mapper;
        }
        public async Task<Response<IEnumerable<GetProductoFondosmaxViewModel>>> Handle(GetProductoFondosmaxQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<IEnumerable<GetProductoFondosmaxViewModel>>();
            try
            {
                var info = await _avirtualRepository.GetProductoFondosmax(request.NroPoliza, request.NroDocumento);
                if (info is not null)
                {
                    response.Data = _mapper.Map<List<GetProductoFondosmaxViewModel>>(info);
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
