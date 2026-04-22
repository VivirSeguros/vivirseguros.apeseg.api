using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using VivirSeguros.CRM.Application.Commons;
using VivirSeguros.CRM.Infrastructure.Persistences.Interfaces;
using VivirSeguros.CRM.Domain.Entities;
using VivirSeguros.CRM.Domain.Common;
using Microsoft.Extensions.Logging;

namespace VivirSeguros.CRM.Application.Services.Jobs.Commands.UpdateJobCommand
{
    public class UpdateJobHandler : IRequestHandler<UpdateJobCommand, Response<bool>>
    {
        private readonly IJobRepository _jobRepository;
        private readonly UpdateJobValidator _validationRules;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateJobHandler> _logger;

        public UpdateJobHandler(IJobRepository jobRepository, IMapper mapper, UpdateJobValidator validationRules,
                                ILogger<UpdateJobHandler> logger)
        {
            _jobRepository = jobRepository;
            _mapper = mapper;
            _validationRules = validationRules;
            _logger = logger;
        }

        public async Task<Response<bool>> Handle(UpdateJobCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<bool>();
            var result = new BaseTransaction();
            try
            {
                var validationResult = _validationRules.Validate(request);

                if (validationResult.IsValid)
                {
                    var job = _mapper.Map<Job>(request);
                    result = await _jobRepository.UpdateAsync(job);

                    if (result.ErrorCode == 0)
                    {
                        response.IsSuccess = true;
                        response.Message = "Actualización exitososa";
                    }
                    else
                    {
                        _logger.LogError(result.Message);
                        response.Message = "Actualización con errores";
                    }
                }
                else
                {
                    response.Message = "Errores en validación de datos";
                    response.Errors = validationResult.Errors;
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
