using MediatR;
using VivirSeguros.CRM.Application.Commons;
using VivirSeguros.CRM.Domain.Entities.Billing;

namespace VivirSeguros.CRM.Application.Services.Billing.Queries.GetInfoBillBySkeyQuery
{
    public class GetInfoBillBySkeyQuery : IRequest<Response<GetInfoBillBySkeyViewModel>>
    {
        public string Skey { get; set; }
    }
}
