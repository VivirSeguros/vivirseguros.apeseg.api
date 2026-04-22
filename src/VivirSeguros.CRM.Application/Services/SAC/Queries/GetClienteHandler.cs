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

namespace VivirSeguros.CRM.Application.Services.SAC.Queries
{
    public class GetClienteHandler : IRequestHandler<GetClienteQuery, Response<IEnumerable<GetClienteViewModel>>>
    {
        private readonly IAVirtualRepository _avirtualRepository;
        private readonly IMapper _mapper;

        public GetClienteHandler(IAVirtualRepository avirtualRepository, IMapper mapper)
        {
            _avirtualRepository = avirtualRepository;
            _mapper = mapper;
        }
        public async Task<Response<IEnumerable<GetClienteViewModel>>> Handle(GetClienteQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<IEnumerable<GetClienteViewModel>>();
            try
            {
                var info = await _avirtualRepository.GetCliente(request.NroPoliza, request.NroDocumento, request.Placa, request.Nombre1, request.ApellidoPaterno);
                if (info is not null)
                {
                    response.Data = _mapper.Map<List<GetClienteViewModel>>(info);
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
