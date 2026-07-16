using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VidaCamara.CrossCuting.Log.Contracts;
using VidaCamara.Domain.Services.Entidades.Configurar;
using VidaCamara.Domain.Services.Repositorios.Configurar;
using VidaCamara.Masivos.Services.Services.Configurar.TipoPerfilModule;
//using VidaCamara.Infrastructure.Data.Configurar;

namespace VidaCamara.Masivos.Services.Services.Configurar.TipoPerfilModule
{
    public class TipoPerfilService : ITipoPerfilService
    {
        private readonly ITipoPerfilRepository perfilRepository;
        private ILoggerManager _logger;
        private IMapper _mapper;

        public TipoPerfilService(ITipoPerfilRepository perfilRepository, ILoggerManager logger, IMapper mapper)
        {
            this.perfilRepository = perfilRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TipoPerfil>> GetTipoPerfil(int param, int prd)
        {
            IEnumerable<TipoPerfil> result = null;
            try {
               var entitieTipoPerfil = await perfilRepository.GetTipoPerfil(param, prd);
                result = _mapper.Map<IEnumerable<TipoPerfil>>(entitieTipoPerfil);
            }                        
            catch (Exception ex){
                throw ex;
            }

            return result;
        }

        public async Task<TipoPerfil> InsTipoPerfil(TipoPerfil param)
        {
            TipoPerfil result = null;
            try
            {
                var entitieTipoPerfil = await perfilRepository.InsTipoPerfil(param);
                result = _mapper.Map<TipoPerfil>(entitieTipoPerfil);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

    }
}
