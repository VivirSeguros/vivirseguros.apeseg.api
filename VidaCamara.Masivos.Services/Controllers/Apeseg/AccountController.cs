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
    public class AccountController : Controller
    {
        private ILoginService _loginService;
        private ILoggerManager _logger;
        private IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppSettings _appSettings;

        public AccountController(ILoggerManager logger,
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
        [HttpPost]
        [Route("GetLogin")]
        public async Task<IActionResult> GetLogin([FromBody] LoginParam _param)
        {
            // 1. Error de validación en el modelo enviado
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "ERROR: Mala petición",
                    token = (string)null,
                    errors = "El modelo de datos no es válido."
                });
            }

            try
            {
                Log.saveFirstLine();
                Log.save(this, "EMPIEZA WEBAPI GetLogin");

                Login datos = await _loginService.GetLogin(_param);

                // 2. El usuario no existe
                if (datos is null)
                {
                    return Ok(new
                    {
                        success = false,
                        message = "username o password incorrecto",
                        token = (string)null,
                        errors = "Las credenciales proporcionadas son incorrectas."
                    });
                }

                // Generar el token (ahora devuelve string directamente)
                string tokenString = BuildToken(datos.login, datos.idPerfil.ToString());
                datos.jwt = tokenString;

                datos.menu = await _loginService.GetMenu(datos.idPerfil);

                Log.save(this, "TERMINA WEBAPI GetLogin");

                // 3. Login Exitoso
                return Ok(new
                {
                    success = true,
                    message = "ok",
                    token = tokenString,
                    errors = (string)null
                });
            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(3).GetFileLineNumber();
                Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);

                // 4. Captura de errores inesperados
                return BadRequest(new
                {
                    success = false,
                    message = "ocurrio error",
                    token = (string)null,
                    errors = ex.Message
                });
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private string BuildToken(string user, string perfil)
        {
            try
            {
                var claims = new[]  {
                    new Claim(ClaimTypes.Name, user),
                    new Claim(JwtRegisteredClaimNames.UniqueName, user),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, perfil)
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

                // Devolvemos únicamente la cadena de texto del JWT compilado
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(3).GetFileLineNumber();
                Log.save(this, "ERROR EN LINEA (BuildToken): " + line + " / " + ex.Message);
                throw;
            }
        }
    }
}