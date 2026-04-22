using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VivirSeguros.CRM.Application.Commons;
using VivirSeguros.CRM.Application.Services.Billing.Commands.SendBillCommand;
using VivirSeguros.CRM.Application.Services.Billing.Queries.GetInfoBillBySkeyQuery;
using VivirSeguros.CRM.Domain.Entities.Billing;

namespace VivirSeguros.CRM.WebApi.Controllers.v1
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BillController : Controller
    {
        private readonly IMediator _mediator;
        public BillController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Operación que permite enviar la información al Servicio Facturador de Econtesch
        /// </summary>
        /// <param name="request"></param>
        /// <param name="sendBillCommand">sendBillCommand</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<ResponseValidacion>), StatusCodes.Status200OK)]
        [HttpPost("SendBillAsync")]
        public async Task<IActionResult> SendBillAsync([FromBody] SendBillCommand request)
        {
            if (request is null)
                return BadRequest();

            var response = await _mediator.Send(request);
            if (response.IsSuccess)
                return Ok(response);
            return BadRequest(response);
        }

        [ProducesResponseType(typeof(Response<GetInfoBillBySkeyViewModel>), StatusCodes.Status200OK)]
        [HttpGet("GetInfoBillBySkeyQuery")]
        public async Task<IActionResult> GetByJobTypeAsync([FromQuery] GetInfoBillBySkeyQuery query)
        {
            var response = await _mediator.Send(query);
            return response.IsSuccess ? Ok(response) : BadRequest(response);

            //if (response.IsSuccess)
            //    return Ok(response);
            //else
            //    return BadRequest(response);
        }
    }
}
