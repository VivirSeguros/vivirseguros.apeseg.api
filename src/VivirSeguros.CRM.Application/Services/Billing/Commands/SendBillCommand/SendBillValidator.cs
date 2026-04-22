using FluentValidation;

namespace VivirSeguros.CRM.Application.Services.Billing.Commands.SendBillCommand
{
    public class SendBillValidator : AbstractValidator<SendBillCommand>
    {
        public SendBillValidator()
        {
            RuleFor(x => x.Skey).NotNull().NotEmpty();
        }
    }
}
