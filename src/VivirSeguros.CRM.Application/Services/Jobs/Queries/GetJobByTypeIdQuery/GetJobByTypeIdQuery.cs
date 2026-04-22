using MediatR;
using System.Collections.Generic;
using VivirSeguros.CRM.Application.Commons;

namespace VivirSeguros.CRM.Application.Services.Jobs.Queries.GetJobByTypeIdQuery
{
    public class GetJobByTypeIdQuery : IRequest<Response<IEnumerable<GetJobByTipeIdViewModel>>>
    {
        public int JobTypeId { get; set; }
    }
}
