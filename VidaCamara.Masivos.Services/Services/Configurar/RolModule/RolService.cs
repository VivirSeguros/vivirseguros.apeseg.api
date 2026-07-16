using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using System.Threading.Tasks;
using VidaCamara.CrossCuting.Log.Contracts;
using VidaCamara.Domain.Services.Repositorios.Configurar;
using VidaCamara.Domain.Services.Entidades.Configurar;

namespace VidaCamara.Masivos.Services.Services.Configurar.RolModule
{
    public class RolService : IRolService
    {
        private readonly IRolRepository rolRepository;
        private ILoggerManager _logger;
        private IMapper _mapper;

        public RolService(IRolRepository rolRepository, ILoggerManager logger, IMapper mapper)
        {
            this.rolRepository = rolRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Rol>> GetRol(int param)
        {
            IEnumerable<Rol> result = null;
            try
            {
                var entitieRol = await rolRepository.GetRol(param);
                result = _mapper.Map<IEnumerable<Rol>>(entitieRol);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public async Task<Rol> InsRol(Rol param)
        {
            Rol result = null;
            try
            {
                var entitieRol = await rolRepository.InsRol(param);
                result = _mapper.Map<Rol>(entitieRol);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }



    }
}

