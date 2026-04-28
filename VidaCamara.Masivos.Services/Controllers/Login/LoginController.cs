using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using VidaCamara.CrossCuting.Log.Contracts;
using VidaCamara.CrossCuting.Utilities.Configuration;
using VidaCamara.Domain.Services.LoginModule;
using VidaCamara.Masivos.Services.Services.LoginModule;
using VidaCamara.Masivos.Services.Extensions;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using VidaCamara.Domain.Services.Entidades.Login;
using VidaCamara.CrossCuting.Utilities;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;


namespace VidaCamara.Masivos.Services.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class LoginController : Controller
    {
        private ILoginService _loginService;
        private ILoggerManager _logger;
        private IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly AppSettings _appSettings;

        public LoginController(ILoggerManager logger,
                               ILoginService loginService,
                               IMapper mapper,
                               IOptions<AppSettings> appSettings,
                               IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _loginService = loginService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _httpContextAccessor = httpContextAccessor;
        }
        

        [AllowAnonymous]
        [HttpGet]
        [Route("ObtenerUsuarioVD")]
        public async Task<IActionResult> ObtenerUsuarioVD()
        {
            return Ok(new { usuario = "USERVIVIR", clave = "U123456" });
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetAmbiente")]
        public async Task<IActionResult> GetAmbiente()
        {
            string aa = Request.GetTypedHeaders().Referer.ToString();
            string host = _httpContextAccessor.HttpContext.Request.Headers["Referer"].ToString();
            Log.save(this, "FACEBOOK 1: " + aa);
            Log.save(this, "FACEBOOK 2: " + host);
            return Ok(new { ambiente = _appSettings.Ambiente });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("GetLogin")]
        public async Task<IActionResult> GetLogin([FromBody] LoginParam _param)
        {
            if (!ModelState.IsValid) return BadRequest(new { mensaje = "ERROR: Mal peticion"});

            try  {
                Log.saveFirstLine();
                Log.save(this, "EMPIEZA WEBAPI GetLogin");
                Login datos = await _loginService.GetLogin(_param);
                if ( datos is null) return Ok(new { mensaje = "Usuario no existe!!" });
                datos.jwt = BuildToken(datos.login, datos.idPerfil.ToString());
                datos.menu = await _loginService.GetMenu(datos.idPerfil);
                Log.save(this, "TERMINA WEBAPI GetLogin");
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
        [Route("GetMenu")]
        public async Task<IActionResult> GetMenu(int param)
        {
            if (!ModelState.IsValid) return BadRequest(new { mensaje = "ERROR: Mal peticion" });

            try   {
                Log.saveFirstLine();
                Log.save(this, "EMPIEZA WEBAPI GetMenu");
                var menu  = await _loginService.GetMenu(param);
                Log.save(this, "TERMINA WEBAPI GetMenu");
                //if (datos is null) return BadRequest(new { mensaje = "ERROR: Usuario no existe" });

                return Ok(new { menu });
            }
            catch (Exception ex)  {
                int line = (new StackTrace(ex, true)).GetFrame(3).GetFileLineNumber();
                Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
                return BadRequest(new { mensaje = ex.Message });
            }
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        private IActionResult BuildToken( string user, string perfil )  
        {
            try {
                var claims = new[]  {
                    new Claim(ClaimTypes.Name, user),
                    new Claim(JwtRegisteredClaimNames.UniqueName, user),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, perfil)
                    //new Claim(ClaimTypes.Role, user.oRol.RolDescripcionRol)
                };

                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                SymmetricSecurityKey symmetricSecuritykey = new SymmetricSecurityKey(key);
                SigningCredentials signingCredentials = new SigningCredentials(symmetricSecuritykey, SecurityAlgorithms.HmacSha256);

                int hora = _appSettings.HoraToken;
                var expiration = DateTime.Now.AddHours(hora);
                var token = new JwtSecurityToken(
                        issuer: _appSettings.Site,
                        audience: _appSettings.Site,
                        expires: expiration,
                        signingCredentials: signingCredentials,
                        claims: claims
                    );
                return Ok(new {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration
                }); 
            }
            catch (Exception ex)   {
                int line = (new StackTrace(ex, true)).GetFrame(3).GetFileLineNumber();
                Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
                throw ex;
            }

        }

        [Authorize]
        [HttpPost]
        [Route("GetCombo")]
        public async Task<IActionResult> GetCombo([FromBody] ComboParam _param)
        {
            if (!ModelState.IsValid) return BadRequest(new { mensaje = "ERROR: Mal peticion" });

            try
            {
                var datos = await _loginService.GetCombo(_param);
                if (datos is null) return Ok(new { mensaje = "ERROR: Tabla para combo no existe" });

                return Ok(new { mensaje = "OK", datos });
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
