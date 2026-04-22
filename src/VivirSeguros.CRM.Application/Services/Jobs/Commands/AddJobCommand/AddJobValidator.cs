using FluentValidation;

namespace VivirSeguros.CRM.Application.Services.Jobs.Commands.AddJobCommand
{
    public class AddJobValidator : AbstractValidator<AddJobCommand>
    {
        public AddJobValidator()
        {
            RuleFor(x => x.ProductId).NotNull().NotEmpty();
            RuleFor(x => x.TypeJobId).NotNull().NotEmpty();
            RuleFor(x => x.PaymentId).NotNull().NotEmpty();
            RuleFor(x => x.State).NotNull().NotEmpty();
        }
    }
}
