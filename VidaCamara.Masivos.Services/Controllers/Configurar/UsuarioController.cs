using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using VidaCamara.CrossCuting.Log.Contracts;
using VidaCamara.CrossCuting.Utilities.Configuration;
using VidaCamara.Masivos.Services.Services.Configurar;
using VidaCamara.Masivos.Services.Services.Configurar.UsuarioModule;
using VidaCamara.Domain.Services.Entidades.Configurar;
using VidaCamara.CrossCuting.Utilities;
using System.Diagnostics;

namespace VidaCamara.Masivos.Services.Controllers.Configurar
{
    [Route("api/[controller]")]
    [Authorize]
    public class UsuarioController : Controller
    {
        private IUsuarioService _usuarioService;
        private ILoggerManager _logger;
        private IMapper _mapper;

        private readonly AppSettings _appSettings;

        public UsuarioController(ILoggerManager logger,
                               IUsuarioService usuarioService,
                               IMapper mapper,
                               IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _usuarioService = usuarioService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [Authorize]
        [HttpGet]
        [Route("GetUsuario/{_param:int}")]
        public async Task<IActionResult> GetUsuario( int _param)
        {
            if (!ModelState.IsValid) return BadRequest(new { mensaje = "ERROR: Mal peticion" });

            try {
                Log.save(this, "EMPIEZA WEBAPI GetUsuario");
                var datos = await _usuarioService.GetUsuario(_param);
                Log.save(this, "TERMINA WEBAPI GetUsuario");
                if (datos is null) return BadRequest(new { mensaje = "ERROR: Usuario no existe" });

                return Ok(new { mensaje = "OK", datos });
            }
            catch (Exception ex)  {
                int line = (new StackTrace(ex, true)).GetFrame(3).GetFileLineNumber();
                Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [Authorize]
        [HttpPost]
        [Route("InsUsuario")]
        public async Task<IActionResult> InsUsuario([FromBody] Usuario _param)
        {
            if (!ModelState.IsValid) return BadRequest(new { mensaje = "ERROR: Mal peticion" });

            try {
                Log.save(this, "EMPIEZA WEBAPI InsUsuario");
                var datos = await _usuarioService.InsUsuario(_param);
                Log.save(this, "TERMINA WEBAPI InsUsuario");
                if (datos.result == 0) return BadRequest(new { mensaje = "ERROR: Error en grabacion" });

                return Ok(new { mensaje = "OK", datos });
            }
            catch (Exception ex)  {
                int line = (new StackTrace(ex, true)).GetFrame(3).GetFileLineNumber();
                Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [Authorize]
        [HttpGet]
        [Route("ValidateExisteUsuario")]
        public async Task<IActionResult> ValidateExisteUsuario(string userName)
        {
            if (!ModelState.IsValid) return BadRequest(new { mensaje = "ERROR: Mal peticion" });

            try
            {
                Log.save(this, "EMPIEZA WEBAPI ValidateExisteUsuario");
                var datos = await _usuarioService.ValidateExisteUsuario(userName);
                Log.save(this, "TERMINA WEBAPI ValidateExisteUsuario");

                return Ok(new { result = datos });
            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(3).GetFileLineNumber();
                Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}

