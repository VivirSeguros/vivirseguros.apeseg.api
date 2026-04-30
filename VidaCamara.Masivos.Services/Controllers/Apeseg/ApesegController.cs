using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using VidaCamara.CrossCuting.Log.Contracts;
using VidaCamara.CrossCuting.Utilities;
using VidaCamara.CrossCuting.Utilities.Configuration;
using VidaCamara.Domain.Services.Entidades.Apeseg;
using VidaCamara.Masivos.Services.Services.Apeseg;

namespace VidaCamara.Masivos.Services.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class ApesegController : Controller
    {
        private readonly IApesegService _apesegService;
        private readonly AppSettings _appSettings;

        public ApesegController(IApesegService apesegService,
                               IOptions<AppSettings> appSettings)
        {
            _apesegService = apesegService;
            _appSettings = appSettings.Value;
        }

        [Authorize]
        [HttpPost]
        [Route("Apeseg_Registrar")]
        public async Task<IActionResult> Apeseg_Registrar([FromBody] RegistroSOATRequest _param)
        {
            if (!ModelState.IsValid) return BadRequest(new { mensaje = "ERROR: Mala petición" });

            try
            {
                Log.saveFirstLine();
                Log.save(this, "EMPIEZA GRABACIÓN A APESEG REGISTRAR PLACA= " + _param.PlacaVehiculo);

                var datos = await _apesegService.Apeseg_Registrar(_param);

                Log.save(this, "TERMINA GRABACIÓN A APESEG REGISTRAR PLACA= " + _param.PlacaVehiculo);

                if (!datos.OperacionExitosa)
                    return BadRequest(new { mensaje = "ERROR: Error en grabación placa " + _param.PlacaVehiculo, errores = datos.CodigoError });

                return Ok(new { mensaje = "OK", datos });
            }
            catch (Exception ex)
            {
                Log.save(this, "ERROR: " + ex.Message);
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [Authorize]
        [HttpPut]
        [Route("Apeseg_Actualizar")]
        public async Task<IActionResult> Apeseg_Actualizar([FromBody] ModificarSOATRequest _param)
        {
            if (!ModelState.IsValid) return BadRequest(new { mensaje = "ERROR: Mala petición" });

            try
            {
                Log.saveFirstLine();
                Log.save(this, "EMPIEZA MODIFICACIÓN A APESEG PLACA= " + _param.PlacaVehiculo);

                var datos = await _apesegService.Apeseg_Actualizar(_param);

                Log.save(this, "TERMINA MODIFICACIÓN A APESEG PLACA= " + _param.PlacaVehiculo);

                if (!datos.OperacionExitosa)
                    return BadRequest(new { mensaje = "ERROR: Error en modificación placa " + _param.PlacaVehiculo, errores = datos.CodigoError });

                return Ok(new { mensaje = "OK", datos });
            }
            catch (Exception ex)
            {
                Log.save(this, "ERROR: " + ex.Message);
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [Authorize]
        [HttpPut]
        [Route("Apeseg_Anular")]
        public async Task<IActionResult> Apeseg_Anular([FromBody] AnulacionSOATRequest _param)
        {
            if (!ModelState.IsValid) return BadRequest(new { mensaje = "ERROR: Mala petición" });

            try
            {
                Log.saveFirstLine();
                Log.save(this, "EMPIEZA ANULACIÓN A APESEG CERTIFICADO= " + _param.polizaCertificado);

                var datos = await _apesegService.Apeseg_Anular(_param);

                Log.save(this, "TERMINA ANULACIÓN A APESEG CERTIFICADO= " + _param.polizaCertificado);

                if (!datos.OperacionExitosa)
                    return BadRequest(new { mensaje = "ERROR: Error en anulación certificado " + _param.polizaCertificado, errores = datos.CodigoError });

                return Ok(new { mensaje = "OK", datos });
            }
            catch (Exception ex)
            {
                Log.save(this, "ERROR: " + ex.Message);
                return BadRequest(new { mensaje = ex.Message });
            }
        }
     
        [Authorize]
        [HttpGet]
        [Route("Consultar/{placa}/{subscriptionKey?}")]
        public async Task<IActionResult> Consultar(string placa, string subscriptionKey = null)
        {
            try
            {
                Log.saveFirstLine();
                Log.save(this, "EMPIEZA Consultar A APESEG / _placa= " + placa);

                ConsultaSOATRequest request = new ConsultaSOATRequest
                {
                    placa = placa,
                    subscriptionKey = subscriptionKey
                };

                var datos = await _apesegService.Consultar(request);

                Log.save(this, "TERMINA Consultar A APESEG / _placa= " + placa);

                if (datos is null) return BadRequest(new { mensaje = "ERROR: No se obtuvo respuesta de APESEG" });

                if (datos.Certificados != null && datos.Certificados.Any())
                {
                    // Lógica para obtener el certificado más reciente si hay varios
                    var filter = datos.Certificados.Count == 1
                                 ? datos.Certificados.First()
                                 : datos.Certificados.Where(x => x.FechaFin != null)
                                                   .OrderByDescending(x => DateTime.Parse(x.FechaFin))
                                                   .FirstOrDefault();

                    if (filter != null)
                    {
                        filter.FechaInicioD = string.IsNullOrEmpty(filter.FechaInicio) ? null : Convert.ToDateTime(filter.FechaInicio);
                        filter.FechaFinD = string.IsNullOrEmpty(filter.FechaFin) ? null : Convert.ToDateTime(filter.FechaFin);

                        // Limpiamos la lista y devolvemos solo el filtrado
                        datos.Certificados.Clear();
                        datos.Certificados.Add(filter);
                    }
                }

                return Ok(new { mensaje = "OK", datos });
            }
            catch (Exception ex)
            {
                Log.save(this, "ERROR: " + ex.Message);
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}