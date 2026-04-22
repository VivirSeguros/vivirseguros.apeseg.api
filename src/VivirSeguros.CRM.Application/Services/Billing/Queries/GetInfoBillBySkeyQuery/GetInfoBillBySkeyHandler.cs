using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VivirSeguros.CRM.Application.Commons;
using VivirSeguros.CRM.Domain.Entities.Billing;
using VivirSeguros.CRM.Infrastructure.Persistences.Interfaces;

namespace VivirSeguros.CRM.Application.Services.Billing.Queries.GetInfoBillBySkeyQuery
{
    public class GetInfoBillBySkeyHandler : IRequestHandler<GetInfoBillBySkeyQuery, Response<GetInfoBillBySkeyViewModel>>
    {
        private readonly IBillRepository _billRepository;
        private readonly IMapper _mapper;

        public GetInfoBillBySkeyHandler(IBillRepository billRepository, IMapper mapper)
        {
            _billRepository = billRepository;
            _mapper = mapper;
        }

        public async Task<Response<GetInfoBillBySkeyViewModel>> Handle(GetInfoBillBySkeyQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<GetInfoBillBySkeyViewModel>();
            try
            {
                var info = await _billRepository.GetAsync(request.Skey);
                if (info is not null)
                {
                    response.Data = _mapper.Map<GetInfoBillBySkeyViewModel>(info);
                    //response.Data = info;
                    response.IsSuccess = true;
                    response.Message = "Consulta Exitosa";
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
