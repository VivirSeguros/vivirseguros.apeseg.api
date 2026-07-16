using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VidaCamara.CrossCuting.Log.Contracts;
using VidaCamara.Domain.Services.Entidades.Configurar;
using VidaCamara.Domain.Services.Repositorios.Configurar;
using VidaCamara.Masivos.Services.Services.Configurar.UsuarioModule;

namespace VidaCamara.Masivos.Services.Services.Configurar.UsuarioModule
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository usuarioRepository;
        private ILoggerManager _logger;
        private IMapper _mapper;

        public UsuarioService(IUsuarioRepository usuarioRepository, ILoggerManager logger, IMapper mapper)
        {
            this.usuarioRepository = usuarioRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Usuario>> GetUsuario(int param)
        {
            IEnumerable<Usuario> result = null;
            try
            {
                var entitieUsuario = await usuarioRepository.GetUsuario(param);
                result = _mapper.Map<IEnumerable<Usuario>>(entitieUsuario);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public async Task<Usuario> InsUsuario(Usuario param)
        {
            Usuario result = null;
            try
            {
                if (param.activeDirectory)
                {
                    param.password = null;
                }
                var entitieUsuario = await usuarioRepository.InsUsuario(param);
                result = _mapper.Map<Usuario>(entitieUsuario);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public async Task<bool> ValidateExisteUsuario(string userName)
        {
            bool result = false;
            try
            {
                result = await usuarioRepository.ValidateExisteUsuario(userName);
            
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

    }
}
