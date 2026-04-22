using MediatR;
using VivirSeguros.CRM.Application.Commons;

namespace VivirSeguros.CRM.Application.Services.Jobs.Commands.AddJobCommand
{
    public class AddJobCommand : IRequest<Response<bool>>
    {
        public int ProductId { get; set; }
        public int TypeJobId { get; set; }
        public int PaymentId { get; set; }
        public string State { get; set; }
        public string CreationUser { get; set; }

    }
}
