using AutoMapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using VidaCamara.CrossCuting.Log.Contracts;
using VidaCamara.CrossCuting.Utilities;
using VidaCamara.Domain.Services.Entidades;
using VidaCamara.Domain.Services.Entidades.Common;
using VidaCamara.Domain.Services.Entidades.Helper;
using VidaCamara.Domain.Services.Repositorios.Helper;

namespace VidaCamara.Masivos.Services.Services.Helper
{
    public class HelperService : IHelperService
    {
        private readonly IHelperRepository _helperRepository;
        private ILoggerManager _logger;
        private IMapper _mapper;
        private readonly IConfiguration _configuration;
        public HelperService(IHelperRepository helperRepository, ILoggerManager logger, IMapper mapper, IConfiguration configuration)
        {
            this._helperRepository = helperRepository;
            _logger = logger;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<IEnumerable<Combo>> GetCombo(int comboId, int ventaDigital, int filtro, int filtro1)
        {
            IEnumerable<Combo> result = null;
            try
            {
                var entitieCombo = await _helperRepository.GetCombo(comboId, ventaDigital, filtro, filtro1);

                result = _mapper.Map<IEnumerable<Combo>>(entitieCombo);
            }
            catch (Exception ex)
            {
                Log.error(this, ex.Message);
                throw ex;
            }
            return result;
        }


        public async Task<string> GetValorTablaConfig(string skey)
        {
            string response = "";
            try
            {
                response = await _helperRepository.GetValorTablaConfig(skey);
            }
            catch (Exception ex)
            {
                Log.error(this, ex.Message);
                throw ex;
            }

            return await Task.FromResult<string>(response);
        }

        public void GuardarDocumentoLog(int autoId, string descripcion, string error, string urlDocumento, int paso, int tipoLog)
        {
            try
            {
                LogTransacParam request = new LogTransacParam();
                request.IdAuto = autoId;
                request.Descripcion = descripcion;
                request.Error = error;
                request.UrlDocumento = urlDocumento;
                request.Paso = paso;
                request.TipoLog = tipoLog;
                _helperRepository.GuardarDocumentoLog(request);
            }
            catch (Exception ex)
            {
                Log.error(this, ex.Message);
                throw ex;
            }
        }


      

    }
}
