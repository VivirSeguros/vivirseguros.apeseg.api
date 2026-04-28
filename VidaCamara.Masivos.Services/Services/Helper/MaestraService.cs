using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VidaCamara.CrossCuting.Log.Contracts;
using VidaCamara.Domain.Services.Entidades.Helper;
using VidaCamara.Domain.Services.Repositorios.Helper;

namespace VidaCamara.Masivos.Services.Services.Helper
{
    public class MaestraService : IMaestraService
    {
        private readonly IMaestraRepository _maestraRepository;
        private ILoggerManager _logger;
        private IMapper _mapper;

        public MaestraService(IMaestraRepository maestraRepository, ILoggerManager logger, IMapper mapper)
        {
            this._maestraRepository = maestraRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Maestra>> GetTipoCanal_SEL()
        {
            IEnumerable<Maestra> response;
            try
            {
                response = await _maestraRepository.GetTipoCanal_SEL();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
        }

        public async Task<IEnumerable<Maestra>> TipoDocumento_sel(MaestraParam idMaestra)
        {
            IEnumerable<Maestra> response;
            try
            {
                response = await _maestraRepository.TipoDocumento_sel(idMaestra);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
        }

        public async Task<IEnumerable<Maestra>> TipoPersona_sel()
        {
            IEnumerable<Maestra> response;
            try
            {
                response = await _maestraRepository.TipoPersona_sel();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
        }
    }

       
    
}
