using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VivirSeguros.CRM.Application.Services.Cuenta.Queries;

namespace VivirSeguros.CRM.WebApi.Controllers.v1
{
    [Authorize]
    [Route("api/v1/[controller]")]
    public class ValidaCuentaController : Controller
    {
        private readonly IMediator _mediator;
        public ValidaCuentaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("ValidaCuenta")]
        public async Task<IActionResult> ValidaCuenta([FromBody] CuentaQuery query)
        {
            var response = await _mediator.Send(query);
            return response.IsSuccess ? Ok(response) : BadRequest(response);

        }
    }
}
