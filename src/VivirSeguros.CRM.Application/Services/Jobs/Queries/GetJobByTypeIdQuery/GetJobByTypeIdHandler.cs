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
using VivirSeguros.CRM.Infrastructure.Persistences.Interfaces;
using static VivirSeguros.CRM.Domain.Extensions.EnumExtensions;

namespace VivirSeguros.CRM.Application.Services.Jobs.Queries.GetJobByTypeIdQuery
{
    public class GetJobByTypeIdHandler : IRequestHandler<GetJobByTypeIdQuery, Response<IEnumerable<GetJobByTipeIdViewModel>>>
    {
        private readonly IJobRepository _jobRepository;
        private readonly IMapper _mapper;

        public GetJobByTypeIdHandler(IJobRepository jobRepository, IMapper mapper)
        {
            _jobRepository = jobRepository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<GetJobByTipeIdViewModel>>> Handle(GetJobByTypeIdQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<IEnumerable<GetJobByTipeIdViewModel>>();
            try
            {
                var info = await _jobRepository.GetByJobTypeAsync(request.JobTypeId);
                if (info is not null)
                {
                    response.Data = _mapper.Map<List<GetJobByTipeIdViewModel>>(info);
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
