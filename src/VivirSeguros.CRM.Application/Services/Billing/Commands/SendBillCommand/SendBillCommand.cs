using MediatR;
using VivirSeguros.CRM.Application.Commons;
using VivirSeguros.CRM.Domain.Entities.Billing;

namespace VivirSeguros.CRM.Application.Services.Billing.Commands.SendBillCommand
{
    public class SendBillCommand : IRequest<Response<ResponseValidacion>>
    {
        public string Skey { get; set; }
    }
}
