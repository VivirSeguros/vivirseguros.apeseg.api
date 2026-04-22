using FluentValidation;

namespace VivirSeguros.CRM.Application.Services.Accounts.Commands.AddTokenCommand
{
    public class AddTokenValidator : AbstractValidator<AddTokenCommand>
    {
        public AddTokenValidator()
        {
            /*RuleFor(x => x.UserName).NotNull().NotEmpty().MinimumLength(5);
            RuleFor(x => x.Password).NotNull().NotEmpty().MinimumLength(5);*/
        }
    }
}
