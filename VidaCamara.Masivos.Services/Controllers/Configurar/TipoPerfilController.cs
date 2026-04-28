using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using VidaCamara.CrossCuting.Log.Contracts;
using VidaCamara.CrossCuting.Utilities.Configuration;
using VidaCamara.Masivos.Services.Services.Configurar;
using VidaCamara.Masivos.Services.Services.Configurar.TipoPerfilModule;
using VidaCamara.Domain.Services.Entidades.Configurar;
using System.Collections.Generic;
using ClosedXML.Excel;
using System.IO;
using VidaCamara.Domain.Services.Entidades;
using VidaCamara.CrossCuting.Utilities;
using System.Diagnostics;

namespace VidaCamara.Masivos.Services.Controllers.Configurar
{
    [Route("api/[controller]")]
    [Authorize]
    public class TipoPerfilController : Controller
    {
        private ITipoPerfilService _perfilService;
        private ILoggerManager _logger;
        private IMapper _mapper;

        private readonly AppSettings _appSettings;

        public TipoPerfilController(ILoggerManager logger,
                               ITipoPerfilService perfilService,
                               IMapper mapper,
                               IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _perfilService = perfilService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [Authorize]
        [HttpGet]
        [Route("GetTipoPerfil/{_param:int}/{_prd:int}")]
        public async Task<IActionResult> GetTipoPerfil( int _param, int _prd)
        {
            if (!ModelState.IsValid) return BadRequest(new { mensaje = "ERROR: Mal peticion" });

            try {
                Log.saveFirstLine();
                Log.save(this, "EMPIEZA WEBAPI GetTipoPerfil");
                var datos = await _perfilService.GetTipoPerfil(_param, _prd);
                Log.save(this, "TERMINA WEBAPI GetTipoPerfil");
                if (datos is null) return BadRequest(new { mensaje = "ERROR: Tipo de Perfil no existe" });

                return Ok(new { mensaje = "OK", datos });
            }
            catch (Exception ex) {
                int line = (new StackTrace(ex, true)).GetFrame(3).GetFileLineNumber();
                Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [Authorize]
        [HttpPost]
        [Route("InsTipoPerfil")]
        public async Task<IActionResult> InsTipoPerfil([FromBody] TipoPerfil _param)
        {
            
            if (!ModelState.IsValid) return BadRequest(new { mensaje = "ERROR: Mal peticion" });

            try
            {
                Log.save(this, "EMPIEZA WEBAPI InsTipoPerfil");
                var datos = await _perfilService.InsTipoPerfil(_param);
                Log.save(this, "TERMINA WEBAPI InsTipoPerfil");
                if (datos.result == 0) return BadRequest(new { mensaje = "ERROR: Error en grabacion" });

                return Ok(new { mensaje = "OK", datos });
            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(3).GetFileLineNumber();
                Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("PrintExcelPerfil")]
        public IActionResult PrintExcelPerfil([FromBody] List<TipoPerfil> request)
        {
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "Perfil.xlsx";
            try
            {
                using (var workbook = new XLWorkbook())
                {
                    Log.save(this, "EMPIEZA WEBAPI PrintExcelPerfil");
                    IXLWorksheet worksheet = workbook.Worksheets.Add("Rol");
                    worksheet.Cell(1, 1).Value = "IdPerfil";
                    worksheet.Cell(1, 2).Value = "Marca";
                    worksheet.Cell(1, 3).Value = "U. Creación";
                    worksheet.Cell(1, 4).Value = "F. Creación";
                    worksheet.Cell(1, 5).Value = "U. Modificación";
                    worksheet.Cell(1, 6).Value = "Estado";
                    for (int index = 1; index <= request.Count; index++)
                    {
                        worksheet.Cell(index + 1, 1).Value = request[index - 1].idTipoPerfil;
                        worksheet.Cell(index + 1, 2).Value = request[index - 1].descripcion;
                        worksheet.Cell(index + 1, 3).Value = request[index - 1].usuarioCreacion;
                        worksheet.Cell(index + 1, 4).Value = request[index - 1].fechaRegistro;
                        //worksheet.Cell(index + 1, 5).Value = request[index - 1].usuarioModificacion;
                        worksheet.Cell(index + 1, 6).Value = request[index - 1].estado == 1 ? "Activo" : "Inactivo";
                    }

                    worksheet.Columns().AdjustToContents();
                    worksheet.Rows().AdjustToContents();
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return File(content, contentType, fileName);
                    }
                    Log.save(this, "TERMINA WEBAPI PrintExcelPerfil");
                }
            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(3).GetFileLineNumber();
                Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
                ErrorVM error = new ErrorVM();
                error.errorCode = 1;
                error.message = ex.Message;
                return Ok(error);
            }
        }
    }
}
