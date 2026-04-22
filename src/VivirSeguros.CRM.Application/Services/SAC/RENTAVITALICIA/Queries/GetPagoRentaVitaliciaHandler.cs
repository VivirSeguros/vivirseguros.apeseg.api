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

namespace VivirSeguros.CRM.Application.Services.SAC.RENTAVITALICIA.Queries
{
    public class GetPagoRentaVitaliciaHandler : IRequestHandler<GetPagoRentaVitaliciaQuery, Response<IEnumerable<GetPagoRentaVitaliciaViewModel>>>
    {

        private readonly IAVirtualRepository _avirtualRepository;
        private readonly IMapper _mapper;

        public GetPagoRentaVitaliciaHandler(IAVirtualRepository avirtualRepository, IMapper mapper)
        {
            _avirtualRepository = avirtualRepository;
            _mapper = mapper;
        }
        public async Task<Response<IEnumerable<GetPagoRentaVitaliciaViewModel>>> Handle(GetPagoRentaVitaliciaQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<IEnumerable<GetPagoRentaVitaliciaViewModel>>();
            try
            {
                var info = await _avirtualRepository.GetPagoRentaVitalicia(request.NroPoliza, request.NroDocumento);
                if (info is not null)
                {
                    response.Data = _mapper.Map<List<GetPagoRentaVitaliciaViewModel>>(info);
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
