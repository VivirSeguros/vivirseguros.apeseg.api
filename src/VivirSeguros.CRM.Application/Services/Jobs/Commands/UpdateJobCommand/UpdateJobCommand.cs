using MediatR;
using VivirSeguros.CRM.Application.Commons;

namespace VivirSeguros.CRM.Application.Services.Jobs.Commands.UpdateJobCommand
{
    public class UpdateJobCommand : IRequest<Response<bool>>
    {
        public int ProductId { get; set; }
        public int TypeJobId { get; set; }
        public string Skey { get; set; }
        public string State { get; set; }
        public string CreationUser { get; set; }
        public JobDetailViewModel DetailJob { get; set; } = new JobDetailViewModel();
    }
}
