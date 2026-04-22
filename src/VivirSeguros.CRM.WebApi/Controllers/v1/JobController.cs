using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VivirSeguros.CRM.Application.Commons;
using VivirSeguros.CRM.Application.Services.Jobs.Commands.AddJobCommand;
using VivirSeguros.CRM.Application.Services.Jobs.Commands.UpdateJobCommand;
using VivirSeguros.CRM.Application.Services.Jobs.Queries.GetJobByTypeIdQuery;

namespace VivirSeguros.CRM.WebApi.Controllers.v1
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class JobController : Controller
    {
        private readonly IMediator _mediator;
        public JobController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Operación que permite crear un nuevo Job
        /// </summary>
        /// <param name="request"></param>
        /// <param name="addJobCommand">addAccountCommand</param>
        /// <returns></returns>
        [HttpPost("InsertAsync")]
        public async Task<IActionResult> InsertAsync([FromBody] AddJobCommand request)
        {
            if (request is null)
                return BadRequest();

            var response = await _mediator.Send(request);
            if (response.IsSuccess)
                return Ok(response);
            return BadRequest(response);
        }

        /// <summary>
        /// Operación que permite actualizar un Job
        /// </summary>
        /// <param name="request"></param>
        /// <param name="updateJobCommand">updateJobCommand</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        [HttpPut("UpdateAsync")]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateJobCommand request)
        {
            if (request is null)
                return BadRequest();

            var response = await _mediator.Send(request);
            if (response.IsSuccess)
                return Ok(response);
            return BadRequest(response);
        }


        [HttpGet("GetByJobTypeAsync")]
        public async Task<IActionResult> GetByJobTypeAsync([FromQuery] GetJobByTypeIdQuery query)
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
