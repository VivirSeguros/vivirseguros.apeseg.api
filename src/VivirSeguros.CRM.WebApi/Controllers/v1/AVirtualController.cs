using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VivirSeguros.CRM.Application.Commons;
using VivirSeguros.CRM.Application.Services.Producto.Queries;
using Microsoft.AspNetCore.Http;
using VivirSeguros.CRM.Application.Services.Endoso.Queries;
using Newtonsoft;
using Newtonsoft.Json;
using VivirSeguros.CRM.Application.Services.SAC.Queries;
using VivirSeguros.CRM.Application.Services.SAC.SOAT.Queries;
using VivirSeguros.CRM.Application.Services.SAC.VIVEMAX.Queries;
using VivirSeguros.CRM.Application.Services.SAC.FONDOSMAX.Queries;
using VivirSeguros.CRM.Application.Services.SAC.RENTAMAX.Queries;
using VivirSeguros.CRM.Application.Services.SAC.RENTAVITALICIA.Queries;

namespace VivirSeguros.CRM.WebApi.Controllers.v1
{
    [Authorize]
    [Route("api/v1/[controller]")]
    public class AVirtualController : Controller
    {
        private readonly IMediator _mediator;
        public AVirtualController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("GetProducto")]
        public async Task<IActionResult> GetProducto([FromBody] GetProductoQuery query)
        {
            Log.save(this, "VivirSeguros.CRM.WebApi.GetProducto");
            var response = await _mediator.Send(query);
            return response.IsSuccess ? Ok(response) : BadRequest(response);

        }


        [AllowAnonymous]
        [ProducesResponseType(typeof(ResponseEndoso<bool>), StatusCodes.Status200OK)]
        [HttpPost("EndosoOrdinario")]
        public async Task<IActionResult> EndosoOrdinario([FromBody] EndosoOrdinarioCommand request)
        {
            Log.save(this, "VivirSeguros.CRM.WebApi.Controllers.v1");
            Log.save(this, "JSON AGENCIA VIRTUAL: "+ JsonConvert.SerializeObject(request));

            if (request is null)
                return BadRequest();

            var response = await _mediator.Send(request);
            if (response.IsSuccess)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPost("GetCliente")]
        public async Task<IActionResult> GetCliente([FromBody] GetClienteQuery query)
        {
            Log.save(this, "Inicio GetCliente");
            var response = await _mediator.Send(query);
            return response.IsSuccess ? Ok(response) : BadRequest(response);

        }

        #region Soat 

        [HttpPost("GetPolizaSoat")]
        public async Task<IActionResult> GetPolizaSoat([FromBody] GetPolizaSoatQuery query)
        {
            Log.save(this, "Inicio GetPolizaSoat");
            var response = await _mediator.Send(query);
            return response.IsSuccess ? Ok(response) : BadRequest(response);

        }


        [HttpPost("GetVehiculoSoat")]
        public async Task<IActionResult> GetVehiculoSoat([FromBody] GetVehiculoSoatQuery query)
        {
            Log.save(this, "Inicio GetVehiculoSoat");
            var response = await _mediator.Send(query);
            return response.IsSuccess ? Ok(response) : BadRequest(response);

        }


        [HttpPost("GetClienteSoat")]
        public async Task<IActionResult> GetClienteSoat([FromBody] GetClienteSoatQuery query)
        {
            Log.save(this, "Inicio GetClienteSoat");
            var response = await _mediator.Send(query);
            return response.IsSuccess ? Ok(response) : BadRequest(response);

        }

        #endregion

        #region Vivemax


        [HttpPost("GetClienteVivemax")]
        public async Task<IActionResult> GetClienteVivemax([FromBody] GetClienteVivemaxQuery query)
        {
            Log.save(this, "Inicio GetClienteVivemax");
            var response = await _mediator.Send(query);
            return response.IsSuccess ? Ok(response) : BadRequest(response);

        }



        [HttpPost("GetProductoVivemax")]
        public async Task<IActionResult> GetProductoVivemax([FromBody] GetProductoVivemaxQuery query)
        {
            Log.save(this, "Inicio GetProductoVivemax");
            var response = await _mediator.Send(query);
            return response.IsSuccess ? Ok(response) : BadRequest(response);

        }



        [HttpPost("GetPagoVivemax")]
        public async Task<IActionResult> GetPagoVivemax([FromBody] GetPagoVivemaxQuery query)
        {
            Log.save(this, "Inicio GetPagoVivemax");
            var response = await _mediator.Send(query);
            return response.IsSuccess ? Ok(response) : BadRequest(response);

        }





        #endregion


        #region Fondosmax

        [HttpPost("GetClienteFondosmax")]
        public async Task<IActionResult> GetClienteFondosmax([FromBody] GetClienteFondosmaxQuery query)
        {
            Log.save(this, "Inicio GetClienteFondosmax");
            var response = await _mediator.Send(query);
            return response.IsSuccess ? Ok(response) : BadRequest(response);

        }


        [HttpPost("GetBeneficiarioFondosmax")]
        public async Task<IActionResult> GetBeneficiarioFondosmax([FromBody] GetBeneficiarioFondosMaxQuery query)
        {
            Log.save(this, "Inicio GetBeneficiarioFondosmax");
            var response = await _mediator.Send(query);
            return response.IsSuccess ? Ok(response) : BadRequest(response);

        }

        [HttpPost("GetProductoFondosmax")]
        public async Task<IActionResult> GetProductoFondosmax([FromBody] GetProductoFondosmaxQuery query)
        {
            Log.save(this, "Inicio GetProductoFondosmax");
            var response = await _mediator.Send(query);
            return response.IsSuccess ? Ok(response) : BadRequest(response);

        }

        [HttpPost("GetPagoFondosmax")]
        public async Task<IActionResult> GetPagoFondosmax([FromBody] GetPagoFondosmaxQuery query)
        {
            Log.save(this, "Inicio GetPagoFondosmax");
            var response = await _mediator.Send(query);
            return response.IsSuccess ? Ok(response) : BadRequest(response);

        }

        #endregion

        #region Rentamax

        [HttpPost("GetTitularRentamax")]
        public async Task<IActionResult> GetTitularRentamax([FromBody] GetTitularRentamaxQuery query)
        {
            Log.save(this, "Inicio GetTitularRentamax");
            var response = await _mediator.Send(query);
            return response.IsSuccess ? Ok(response) : BadRequest(response);

        }

        [HttpPost("GetAseguradoRentamax")]
        public async Task<IActionResult> GetAseguradoRentamax([FromBody] GetAseguradoRentamaxQuery query)
        {
            Log.save(this, "Inicio GetAseguradoRentamax");
            var response = await _mediator.Send(query);
            return response.IsSuccess ? Ok(response) : BadRequest(response);

        }


        [HttpPost("GetBeneficiarioRentamax")]
        public async Task<IActionResult> GetBeneficiarioRentamax([FromBody] GetBeneficiarioRentamaxQuery query)
        {
            Log.save(this, "Inicio GetBeneficiarioRentamax");
            var response = await _mediator.Send(query);
            return response.IsSuccess ? Ok(response) : BadRequest(response);

        }


        [HttpPost("GetProductoRentamax")]
        public async Task<IActionResult> GetProductoRentamax([FromBody] GetProductoRentamaxQuery query)
        {
            Log.save(this, "Inicio GetProductoRentamax");
            var response = await _mediator.Send(query);
            return response.IsSuccess ? Ok(response) : BadRequest(response);

        }

        #endregion



        #region Rentamax



        [HttpPost("GetTitularRentaVitalicia")]
        public async Task<IActionResult> GetTitularRentaVitalicia([FromBody] GetTitularRentaVitaliciaQuery query)
        {
            Log.save(this, "Inicio GetTitularRentaVitalicia");
            var response = await _mediator.Send(query);
            return response.IsSuccess ? Ok(response) : BadRequest(response);

        }


        [HttpPost("GetProductoRentaVitalicia")]
        public async Task<IActionResult> GetProductoRentaVitalicia([FromBody] GetProductoRentaVitaliciaQuery query)
        {
            Log.save(this, "Inicio GetProductoRentaVitalicia");
            var response = await _mediator.Send(query);
            return response.IsSuccess ? Ok(response) : BadRequest(response);

        }



        [HttpPost("GetPagoRentaVitalicia")]
        public async Task<IActionResult> GetPagoRentaVitalicia([FromBody] GetPagoRentaVitaliciaQuery query)
        {
            Log.save(this, "Inicio GetPagoRentaVitalicia");
            var response = await _mediator.Send(query);
            return response.IsSuccess ? Ok(response) : BadRequest(response);

        }



        [HttpPost("GetBeneficiarioRentaVitalicia")]
        public async Task<IActionResult> GetBeneficiarioRentaVitalicia([FromBody] GetBeneficiarioRentaVitaliciaQuery query)
        {
            Log.save(this, "Inicio GetBeneficiarioRentaVitalicia");
            var response = await _mediator.Send(query);
            return response.IsSuccess ? Ok(response) : BadRequest(response);

        }


        #endregion


 

    }
}
