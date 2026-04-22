using FluentValidation;
namespace VivirSeguros.CRM.Application.Services.Jobs.Commands.UpdateJobCommand
{
     public class UpdateJobValidator : AbstractValidator<UpdateJobCommand>
    {
        public UpdateJobValidator()
        {
            RuleFor(x => x.ProductId).NotNull().NotEmpty();
            RuleFor(x => x.TypeJobId).NotNull().NotEmpty();
            RuleFor(x => x.DetailJob.Request).NotNull().NotEmpty();
            RuleFor(x => x.DetailJob.Request1).NotNull().NotEmpty();
            RuleFor(x => x.DetailJob.Response).NotNull().NotEmpty();
            RuleFor(x => x.DetailJob.ResponseCode).NotNull().NotEmpty();
        }
    }
}
