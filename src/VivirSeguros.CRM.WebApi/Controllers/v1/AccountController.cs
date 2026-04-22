using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MediatR;
using VivirSeguros.CRM.Application.Services.Accounts.Commands.AddAccountCommand;
using VivirSeguros.CRM.Application.Services.Accounts.Commands.AddTokenCommand;
using Microsoft.AspNetCore.Http;
using VivirSeguros.CRM.Application.Commons;

namespace VivirSeguros.CRM.WebApi.Controllers.v1
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Operación que permite crear usuarios CRM partir de un usuario y codigo secreto.
        /// </summary>
        /// <param name="addAccountCommand">addTokenCommand</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        [HttpPost("AccountAsync")]
        public async Task<IActionResult> AccountAsync([FromBody] AddAccountCommand addAccountCommand)
        {
            if (addAccountCommand is null)
                return BadRequest();

            var response = await _mediator.Send(addAccountCommand);
            if (response.IsSuccess)
                return Ok(response);
            return BadRequest(response);
        }

        /// <summary>
        /// Operación que permite generar un TOKEN a partir de un usuario y password.
        /// </summary>
        /// <param name="addTokenCommand">addTokenCommand</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        [HttpPost("TokenAsync")]
        public async Task<IActionResult> TokenAsync([FromBody] AddTokenCommand addTokenCommand)
        {
            if (addTokenCommand is null)
                return BadRequest();

            var response = await _mediator.Send(addTokenCommand);
            if (response.IsSuccess)
                return Ok(response);
            return BadRequest(response);
        }
    }
}
