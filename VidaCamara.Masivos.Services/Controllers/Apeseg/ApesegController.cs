using AutoMapper;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using VidaCamara.CrossCuting.Log.Contracts;
using VidaCamara.CrossCuting.Utilities;
using VidaCamara.CrossCuting.Utilities.Configuration;
using VidaCamara.Domain.Services.Entidades;
using VidaCamara.Domain.Services.Entidades.Apeseg;
using VidaCamara.Masivos.Services.Services.Apeseg;

namespace VidaCamara.Masivos.Services.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class ApesegController : Controller
    {
        private IApesegService _apesegService;
        private ILoggerManager _logger;
        private IMapper _mapper;

        private static HttpClient httpClient = new HttpClient();
        private readonly AppSettings _appSettings;

        public ApesegController(ILoggerManager logger,
                               IApesegService apesegService,
                               IMapper mapper,
                               IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _apesegService = apesegService;
            _mapper = mapper;
            _appSettings = appSettings.Value;

        }

        [Authorize]
        [HttpPost]
        [Route("Apeseg_Send")]
        public async Task<IActionResult> Apeseg_Send([FromBody] ApesegParam _param)
        {

            if (!ModelState.IsValid) return BadRequest(new { mensaje = "ERROR: Mal peticion" });

            try
            {
                Log.saveFirstLine();
                Log.save(this, "EMPIEZA GRABACIÓN A APESEG IDAUTO= " + _param.idAuto);
                var datos = await _apesegService.Apeseg_Send(_param);
                Log.save(this, "TERMINA GRABACIÓN A APESEG IDAUTO= " + _param.idAuto);
                if (!datos.OperacionExitosa) return BadRequest(new { mensaje = "ERROR: Error en grabacion" });

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
        [HttpGet]
        [Route("Consultar/{_placa}")]
        public async Task<IActionResult> Consultar(string _placa)
        {
            if (!ModelState.IsValid) return BadRequest(new { mensaje = "ERROR: Mal peticion" });

            try
            {
                Log.saveFirstLine();
                Log.save(this, "EMPIEZA Consultar A APESEG / _placa= " + _placa);
                var datos = await _apesegService.Consultar(_placa);
                Log.save(this, "TERMINA Consultar A APESEG / _placa= " + _placa);
                if (datos is null) return BadRequest(new { mensaje = "ERROR: Error en grabacion" });

                var i = 0;

                var filter = new Consulta();

                if (datos.Certificados is not null)
                {
                    if (datos.Certificados.Count == 1)
                        filter = datos.Certificados.FirstOrDefault();
                    else
                        filter = datos.Certificados.Where(x => x.FechaFin is not null).OrderByDescending(x => DateTime.Parse(x.FechaFin)).FirstOrDefault();

                    /*for(i = 0; i < datos.Certificados.Count ; i++) {
                        datos.Certificados[i].FechaInicioD = datos.Certificados[i].FechaInicio == null ? null : Convert.ToDateTime(datos.Certificados[i].FechaInicio);
                        datos.Certificados[i].FechaFinD = datos.Certificados[i].FechaFin == null ? null : Convert.ToDateTime(datos.Certificados[i].FechaFin);
                    }*/
                    if (filter is not null)
                    {
                        filter.FechaInicioD = filter.FechaInicio == null ? null : Convert.ToDateTime(filter.FechaInicio);
                        filter.FechaFinD = filter.FechaFin == null ? null : Convert.ToDateTime(filter.FechaFin);
                        List<Consulta> certificados = new List<Consulta>();
                        certificados.Add(filter);
                        datos.Certificados.Clear();
                        datos.Certificados.AddRange(certificados);
                    }
                }


                return Ok(new { mensaje = "OK", datos });
            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        //[Authorize]
        //[HttpPost]
        //[Route("Pago_Send")]
        ////public async Task<IActionResult> Pago_Send([FromBody] PagoParam _param)
        //public async Task<IActionResult> Pago_Send([FromBody] VentaOnlineBaseParam _param)
        //{
        //    OrdenPago orden = new OrdenPago();
        //    PagoParam entry = new PagoParam();
        //    Pago datos = new Pago();
        //    if (!ModelState.IsValid) return BadRequest(new { mensaje = "ERROR: Mal peticion" });
        //    Log.saveFirstLine();
        //    Log.save(this, "EMPIEZA WEBAPI Pago_Send");

        //    var validator = new PagoSendValidator();
        //    ValidationResult result = validator.Validate(_param);

        //    try
        //    {

        //        if (result.IsValid)
        //        {
        //            var izyPay = new HttpClient();
        //            string uri = ""; string jsonRegistro = ""; string respuesta = "";

        //            izyPay.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
        //            izyPay.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(
        //                   System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", _appSettings.IziUsuario, _appSettings.IziClave))));

        //            //----- Guarda Orden 
        //            orden.User = _param.UsuarioCreacion;
        //            orden.idOrdenPago = 0;
        //            orden.Placa = _param.Auto.Placa;
        //            orden.Prima = (decimal)_param.Auto.Prima;
        //            orden.NroDocumento = _param.Persona.NroDocumento;
        //            orden.JsonAuto = JsonConvert.SerializeObject(_param);
        //            await _apesegService.Orden_Pago(orden);
        //            //----- Fin Orden
        //            Log.save(this, "EMPIEZA LEER JSON");
        //            entry.orderId = "SO-" + (orden.idOrdenPago.ToString().PadLeft(10, '0'));
        //            entry.currency = "PEN";
        //            entry.customer.email = _appSettings.IziCorreoActivo.Equals("1") ? _param.Telefono.Email : _appSettings.IziCorreo;
        //            entry.amount = (int)(100 * _param.Auto.Prima);
        //            if (_appSettings.Ambiente == "DESARROLLO") entry.amount = 1;  // Quitar. Es solo para pruebas

        //            jsonRegistro = JsonConvert.SerializeObject(entry);
        //            Log.save(this, "EMPIEZA LEER jsonRegistro" + jsonRegistro);
        //            HttpResponseMessage response;
        //            byte[] byteData = Encoding.UTF8.GetBytes(jsonRegistro);
        //            uri = _appSettings.Izipay;
        //            using (var content = new ByteArrayContent(byteData))
        //            {
        //                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        //                response = await izyPay.PostAsync(uri, content);
        //                respuesta = await response.Content.ReadAsStringAsync();
        //                datos = JsonConvert.DeserializeObject<Pago>(respuesta);

        //            }
        //            Log.save(this, "TERMINA LEER JSON");
        //            Log.save(this, "TERMINA WEBAPI Pago_Send");

        //        }
        //        else
        //        {
        //            string str = "";
        //            foreach (var error in result.Errors) { str += error; }
        //            return BadRequest(new { mensaje = str });
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        int line = (new StackTrace(ex, true)).GetFrame(3).GetFileLineNumber();
        //        Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
        //        return BadRequest(new { mensaje = ex.Message });
        //    }

        //    return Ok(new { datos });
        //}

        //[Authorize]
        //[HttpPost]
        //[Route("Pago_Save")]
        //public async Task<IActionResult> Pago_Save([FromBody] Respuesta _param)
        //{

        //    if (!ModelState.IsValid) return BadRequest(new { mensaje = "ERROR: Mal peticion" });

        //    try
        //    {
        //        Log.saveFirstLine();
        //        Log.save(this, "EMPIEZA WEBAPI Pago_Save");
        //        var datos = await _apesegService.Pago_Save(_param);
        //        //if (!datos.OperacionExitosa) return BadRequest(new { mensaje = "ERROR: Error en grabacion" });
        //        Log.save(this, "TERMINA WEBAPI Pago_Save");
        //        return Ok(new { mensaje = "OK", datos });
        //    }
        //    catch (Exception ex)
        //    {
        //        int line = (new StackTrace(ex, true)).GetFrame(3).GetFileLineNumber();
        //        Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
        //        return BadRequest(new { mensaje = ex.Message });
        //    }
        //}

        //[Authorize]
        //[HttpPost]
        //[Route("ValidaPago")]
        //public async Task<IActionResult> ValidaPago([FromBody] Respuesta _param)
        //{

        //    if (!ModelState.IsValid) return BadRequest(new { mensaje = "ERROR: Mal peticion" });
        //    JSIzipay Contenido = JsonConvert.DeserializeObject<JSIzipay>(_param.formToken);
        //    string Orden = Contenido.clientAnswer.orderDetails.orderId;
        //    decimal Monto = Contenido.clientAnswer.transactions[0].amount / 100;
        //    if (Orden is null)
        //        return BadRequest(new { mensaje = "ERROR: No existe Orden de Pago" });

        //    try
        //    {
        //        Log.saveFirstLine();
        //        Log.save(this, "EMPIEZA WEBAPI ValidaPago");
        //        int existe = await _apesegService.Validar_Pago(Orden, Monto);
        //        Log.save(this, "TERMINA WEBAPI ValidaPago");
        //        if (existe == 1)
        //            return Ok(new { mensaje = "OK", existe });
        //        else
        //            return BadRequest(new { mensaje = "ERROR: No existe Orden de Pago" });
        //    }
        //    catch (Exception ex)
        //    {
        //        int line = (new StackTrace(ex, true)).GetFrame(3).GetFileLineNumber();
        //        Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
        //        return BadRequest(new { mensaje = ex.Message });
        //    }
        //}

        //[Authorize]
        //[HttpPost]
        //[Route("Orden_Pago")]
        //public async Task<IActionResult> Orden_Pago([FromBody] OrdenPago _param)
        //{

        //    if (!ModelState.IsValid) return BadRequest(new { mensaje = "ERROR: Mal peticion" });

        //    try
        //    {
        //        Log.saveFirstLine();
        //        Log.save(this, "EMPIEZA WEBAPI Orden_Pago");
        //        var datos = await _apesegService.Orden_Pago(_param);
        //        Log.save(this, "TERMINA WEBAPI Orden_Pago");
        //        return Ok(new { mensaje = "OK", datos });
        //    }
        //    catch (Exception ex)
        //    {
        //        int line = (new StackTrace(ex, true)).GetFrame(3).GetFileLineNumber();
        //        Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
        //        return BadRequest(new { mensaje = ex.Message });
        //    }
        //}

        //[Authorize]
        //[HttpPost]
        //[Route("GetOrden")]
        //public async Task<IActionResult> GetOrden([FromBody] OrdenPagoParam request)
        //{
        //    IEnumerable<OrdenPago> response;
        //    //response = await _apesegService.GetOrden(request);

        //    if (!ModelState.IsValid) return BadRequest(new { mensaje = "ERROR: Mal peticion" });

        //    try
        //    {
        //        response = await _apesegService.GetOrden(request);
        //        if (response is null) return BadRequest(new { mensaje = "ERROR: Error en grabacion" });

        //        return Ok(new { mensaje = "OK", response });
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.error(this, ex.Message);
        //        ErrorVM error = new ErrorVM();
        //        error.errorCode = 1;
        //        error.message = ex.Message;
        //        return Ok(error);
        //    }

        //    return Ok(response);
        //}

        //[Authorize]
        //[HttpPost]
        //[Route("ConsultarPlacasMasivo")]
        //public async Task<IActionResult> ConsultarPlacasMasivo([FromForm] string data, [FromForm] IFormFile file)
        //{
        //    try
        //    {
        //        var request = JsonConvert.DeserializeObject<ConsultaMasivaRequest>(data);

        //        ConsultaMasivaResponse response = await _apesegService.InsertaProcesoConsultaPlacasMasivo(request);

        //        if (response.idProceso > 0)
        //        {

        //            var lsPlacas = ObtienePlacasExcel(file);

        //            _ = Task.Run(async () =>
        //            {
        //                try
        //                {
        //                    Log.saveFirstLine();
        //                    Log.save(this, "EMPIEZA WEBAPI ConsultarPlacasMasivo ASYNC");
        //                    request.idProceso = response.idProceso;

        //                    await _apesegService.ConsultaPlacasMasivo(request, lsPlacas);

        //                    Log.save(this, "FIN WEBAPI ConsultarPlacasMasivo ASYNC");
        //                }
        //                catch (Exception ex)
        //                {
        //                    int line = (new StackTrace(ex, true)).GetFrame(3).GetFileLineNumber();
        //                    Log.save(this, "ERROR ConsultarPlacasMasivo ASYNC EN LINEA: " + line + " / " + ex.Message);
        //                    request.Mensaje = ex.Message;
        //                    await _apesegService.ActualizaErrorPlacasMasivo(request);
        //                }

        //            });
        //        }

        //        return Ok(new { mensaje = "OK", data = response });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { mensaje = ex.Message });
        //    }

        //}

        //private List<PlacaAPESEG> ObtienePlacasExcel(IFormFile file)
        //{
        //    List<PlacaAPESEG> fileData = new List<PlacaAPESEG>();

        //    if (file != null && file.Length > 0)
        //    {
        //        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        //        using (var package = new ExcelPackage(file.OpenReadStream()))
        //        {
        //            var dataTable = new DataTable();

        //            ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Obtén la primera hoja del archivo

        //            bool hasHeaderRow = true; // Define si el archivo Excel tiene una fila de encabezado

        //            // Recorre las filas del archivo Excel
        //            int startRow = hasHeaderRow ? 2 : 1;
        //            for (int row = startRow; row <= worksheet.Dimension.End.Row; row++)
        //            {
        //                // Si es la primera fila, crea las columnas en el DataTable
        //                if (row == startRow)
        //                {
        //                    for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
        //                    {
        //                        int rowHeader = hasHeaderRow ? 1 : row;
        //                        string columnName = hasHeaderRow ? worksheet.Cells[rowHeader, col].Value?.ToString() : $"Column_{col}";
        //                        dataTable.Columns.Add(columnName);
        //                    }
        //                }

        //                // Lee los valores de cada celda y agrega una nueva fila al DataTable
        //                DataRow dataRow = dataTable.Rows.Add();
        //                for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
        //                {
        //                    dataRow[col - 1] = worksheet.Cells[row, col].Value?.ToString();
        //                }
        //            }

        //            // Ahora tienes los datos del archivo Excel almacenados en el DataTable 'dataTable'
        //            // Puedes realizar las operaciones que necesites con los datos

        //            fileData = (from DataRow r in dataTable.Rows
        //                        where !string.IsNullOrWhiteSpace(r["Placa"].ToString())
        //                        select new PlacaAPESEG { Placa = r["Placa"].ToString() }).ToList();


        //        }

        //    }

        //    return fileData;

        //}


        //[Authorize]
        //[HttpGet]
        //[Route("ConsultarProcesosMasivos")]
        //public async Task<IActionResult> ConsultarProcesosMasivos()
        //{
        //    try
        //    {
        //        List<ApesegConsultaPlacaMasivo> response = await _apesegService.ConsultarProcesosMasivos();


        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { mensaje = ex.Message });
        //    }

        //}
        //[Authorize]
        //[HttpPost]
        //[Route("ConsultarProcesoDetalle")]
        //public async Task<IActionResult> ConsultarProcesoDetalle([FromBody] ConsultaMasivaRequest request)
        //{
        //    try
        //    {
        //        List<ApesegConsultaPlacaMasivoDetalle> response = await _apesegService.ConsultarProcesoDetalle(request);

        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { mensaje = ex.Message });
        //    }

        //}

        //[Authorize]
        //[HttpPost]
        //[Route("CancelaProcesoMasivo")]
        //public async Task<IActionResult> CancelaProcesoMasivo([FromBody] ConsultaMasivaRequest request)
        //{
        //    try
        //    {

        //        request.Estado = "CA"; // Cancela el proceso Masivo

        //        ConsultaMasivaResponse response = await _apesegService.ActualizaProcesoMasivo(request);

        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { mensaje = ex.Message });
        //    }

        //}

        ////[Authorize]
        //[AllowAnonymous]
        //[HttpGet]
        //[Route("InsertaEnvioCarrito/{toque}")]
        //public async Task<IActionResult> InsertaEnvioCarrito(int toque)
        //{
        //    try
        //    {
        //        Log.saveFirstLine();
        //        Log.save(this, $"EMPIEZA WEBAPI InsertaEnvioCarrito Toque:{toque} ");
        //        var request = new EnvioCarritoRequest() { Toque = toque };
        //        EnvioCarritoResponse response = await _apesegService.InsertaEnvioCarrito(request);

        //        Log.save(this, "FIN WEBAPI InsertaEnvioCarrito Toque:{toque} ");

        //        if (response.Placas.Any())
        //        {
        //            response = await _apesegService.ActualizaEnvioCarrito(response.Placas, toque);
        //        }

        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        int line = (new StackTrace(ex, true)).GetFrame(3).GetFileLineNumber();
        //        Log.save(this, "ERROR EN LINEA: " + line + " / " + ex.Message);
        //        return BadRequest(new { mensaje = ex.Message });
        //    }

        //}
    }
}