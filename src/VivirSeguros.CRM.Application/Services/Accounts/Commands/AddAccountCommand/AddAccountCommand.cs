using VivirSeguros.CRM.Application.Commons;
using MediatR;

namespace VivirSeguros.CRM.Application.Services.Accounts.Commands.AddAccountCommand
{
    public class AddAccountCommand : IRequest<Response<bool>>
    {
        public int idUsuario { get; set; }

        public string Username { get; set; }

        public string Secret { get; set; }
    }
}
