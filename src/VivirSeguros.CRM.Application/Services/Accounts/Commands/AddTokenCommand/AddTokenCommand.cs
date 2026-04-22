using VivirSeguros.CRM.Application.Commons;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace VivirSeguros.CRM.Application.Services.Accounts.Commands.AddTokenCommand
{
    public class AddTokenCommand : IRequest<Response<string>>
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Secret { get; set; }
    }
}
