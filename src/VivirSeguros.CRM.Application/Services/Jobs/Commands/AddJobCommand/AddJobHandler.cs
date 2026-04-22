using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using VivirSeguros.CRM.Application.Commons;
using VivirSeguros.CRM.Infrastructure.Persistences.Interfaces;
using VivirSeguros.CRM.Domain.Entities;
namespace VivirSeguros.CRM.Application.Services.Jobs.Commands.AddJobCommand
{
    public class AddJobHandler : IRequestHandler<AddJobCommand, Response<bool>>
    {
        private readonly IJobRepository _jobRepository;
        private readonly AddJobValidator _validationRules;
        private readonly IMapper _mapper;
        public AddJobHandler(IJobRepository jobRepository, IMapper mapper, AddJobValidator validationRules)
        {
            _jobRepository = jobRepository;
            _mapper = mapper;
            _validationRules = validationRules;
        }

        public async Task<Response<bool>> Handle(AddJobCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<bool>();
            try
            {
                var job = _mapper.Map<Job>(request);
                var validationResult = _validationRules.Validate(request);

                if (validationResult.IsValid)
                {
                    response.Data = await _jobRepository.InsertAsync(job);
                    response.IsSuccess = true;
                    response.Message = "Registro exitoso";
                }
                else
                {
                    response.Message = "Registro con errores";
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
