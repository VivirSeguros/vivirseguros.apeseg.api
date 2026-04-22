using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VivirSeguros.CRM.Application.Commons;
using VivirSeguros.CRM.Application.Services.Endoso.Queries;
using VivirSeguros.CRM.Domain.Common;
using VivirSeguros.CRM.Domain.Entities.Producto;
using VivirSeguros.CRM.Infrastructure.Persistences.Interfaces;

namespace VivirSeguros.CRM.Application.Services.Endoso.Commands
{
    public class EndosoOrdinarioHandler : IRequestHandler<EndosoOrdinarioCommand, ResponseEndoso<bool>>
    {
        private readonly IAVirtualRepository _aVirtualRepository;
        //private readonly UpdateJobValidator _validationRules;
        private readonly IMapper _mapper;
        private readonly ILogger<EndosoOrdinarioHandler> _logger;

        public EndosoOrdinarioHandler(IAVirtualRepository aVirtualRepository, IMapper mapper,
                                ILogger<EndosoOrdinarioHandler> logger)
        {
            _aVirtualRepository = aVirtualRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ResponseEndoso<bool>> Handle(EndosoOrdinarioCommand request, CancellationToken cancellationToken)
        {
            var response = new ResponseEndoso<bool>();
            var result = new ResponseTransaction();
            var endosoOrdinario = _mapper.Map<EndosoOrdinario>(request);
            try
            {
                result = await _aVirtualRepository.EndosoOrdinarioAsync(endosoOrdinario);
                //result = await _aVirtualRepository.EndosoOrdinario2Async(endosoOrdinario);

                if (result.ErrorCode == 0)
                {
                    response.IsSuccess = true;
                    response.Message = "Endoso generado exitosamente";
                }
                else
                {
                    _logger.LogError(result.Message);
                    response.Message = "Error al generar Endoso";
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            try
            {
                result = await _aVirtualRepository.EndosoOrdinario2Async(endosoOrdinario);

                if (result.ErrorCode2 == 0)
                {
                    response.IsSuccess2 = true;
                    response.Message2 = "Endoso generado exitosamente";
                }
                else if (result.ErrorCode2 == -1)
                {
                    response.IsSuccess2 = false;
                    response.Message2 = "Persona se encuentra fallecida";
                }
                else
                {
                    _logger.LogError(result.Message2);
                    response.Message2 = "Error al generar Endoso";
                }
            }
            catch (Exception ex)
            {
                response.Message2 = ex.Message;
            }

            try
            {
                result = await _aVirtualRepository.EndosoOrdinario3Async(endosoOrdinario);

                if (result.ErrorCode3 == 0)
                {
                    response.IsSuccess3 = true;
                    response.Message3 = "Endoso generado exitosamente";
                }
                else if (result.ErrorCode3 == -1)
                {
                    response.IsSuccess3 = false;
                    response.Message3 = "Persona se encuentra fallecida";
                }
                else
                {
                    _logger.LogError(result.Message3);
                    response.Message3 = "Error al generar Endoso";
                }
            }
            catch (Exception ex)
            {
                response.Message3 = ex.Message;
            }


            try
            {
                result = await _aVirtualRepository.EndosoOrdinario4Async(endosoOrdinario);

                if (result.ErrorCode4 == 0)
                {
                    response.IsSuccess4 = true;
                    response.Message4 = "Endoso generado exitosamente";
                }
                else if (result.ErrorCode4 == -1)
                {
                    response.IsSuccess4 = false;
                    response.Message4 = "Persona se encuentra fallecida";
                }
                else
                {
                    _logger.LogError(result.Message4);
                    response.Message4 = "Error al generar Endoso";
                }
            }
            catch (Exception ex)
            {
                response.Message4 = ex.Message;
            }


            return response;
        }
    }
}
