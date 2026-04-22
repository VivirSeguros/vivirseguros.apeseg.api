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
    public class GetClienteFondosmaxHandler : IRequestHandler<GetClienteFondosmaxQuery, Response<IEnumerable<GetClienteFondosmaxViewModel>>>
    {
        private readonly IAVirtualRepository _avirtualRepository;
        private readonly IMapper _mapper;

        public GetClienteFondosmaxHandler(IAVirtualRepository avirtualRepository, IMapper mapper)
        {
            _avirtualRepository = avirtualRepository;
            _mapper = mapper;
        }
        public async Task<Response<IEnumerable<GetClienteFondosmaxViewModel>>> Handle(GetClienteFondosmaxQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<IEnumerable<GetClienteFondosmaxViewModel>>();
            try
            {
                var info = await _avirtualRepository.GetClienteFondosmax(request.NroPoliza, request.NroDocumento);
                if (info is not null)
                {
                    response.Data = _mapper.Map<List<GetClienteFondosmaxViewModel>>(info);
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
