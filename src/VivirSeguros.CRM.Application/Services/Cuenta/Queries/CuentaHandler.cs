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


namespace VivirSeguros.CRM.Application.Services.Cuenta.Queries
{
    public class CuentaHandler : IRequestHandler<CuentaQuery, Response<IEnumerable<CuentaViewModel>>>
    {
        private readonly IValidaCuentaRepository _validaCuentaRepository;
        private readonly IMapper _mapper;

        public CuentaHandler(IValidaCuentaRepository validaCuentaRepository, IMapper mapper)
        {
            _validaCuentaRepository = validaCuentaRepository;
            _mapper = mapper;
        }
        public async Task<Response<IEnumerable<CuentaViewModel>>> Handle(CuentaQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<IEnumerable<CuentaViewModel>>();
            try
            {
                var info = await _validaCuentaRepository.ValidaCuenta(request.NroDoc);
                if (info is not null)
                {
                    response.Data = _mapper.Map<List<CuentaViewModel>>(info);
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
