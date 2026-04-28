using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
//using VidaCamara.Masivos.Services.Dtos.Login;
using VidaCamara.CrossCuting.Log.Contracts;
using VidaCamara.Domain.Services.Entidades.Login;
using VidaCamara.Domain.Services.LoginModule;
using System.DirectoryServices;
using VidaCamara.Domain.Services.Repositorios.Helper;

namespace VidaCamara.Masivos.Services.Services.LoginModule
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository loginRepository;
        private readonly IHelperRepository _helperRepository;
        private ILoggerManager _logger;
        private IMapper _mapper;

        public LoginService(ILoginRepository loginRepository, IHelperRepository helperRepository, ILoggerManager logger, IMapper mapper)
        {
            this.loginRepository = loginRepository;
            this._helperRepository = helperRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Login> GetLogin( LoginParam param)
        {
            Login result = null;
            try  {
                bool isAd = await loginRepository.CheckIfAD(param.username);
                if (isAd)
                {
                    string ldap = await _helperRepository.GetValorTablaConfig("LDAPAD");
                    string dominio = await _helperRepository.GetValorTablaConfig("DOMINIOAD");
                    string dominioUsuario = dominio + @"\" + param.username;
                    bool valida = AutenticarUsAd(ldap, dominioUsuario, param.password, param.username);
                    if (valida)
                    {
                        Login entitieLogin = await loginRepository.GetLogin(param);
                        result = _mapper.Map<Login>(entitieLogin);

                    }
                }
                else
                {
                    Login entitieLogin = await loginRepository.GetLogin(param);
                    result = _mapper.Map<Login>(entitieLogin);
                }
               
            }
            catch (Exception ex) {
                throw ex;
            }
            return result;
        }

        public async Task<IEnumerable<Modulo>> GetMenu(int _param)
        {
            IEnumerable<Modulo> result = null;
            try {
                var entitieLogin = await loginRepository.GetMenu(_param);

                result = _mapper.Map<IEnumerable<Modulo>>(entitieLogin); 
            }
            catch (Exception ex) {
                throw ex;
            }
            return result;
        }
        public async Task<IEnumerable<Combo>> GetCombo(ComboParam param)
        {
            IEnumerable<Combo> result = null;
            try  {
                var entitieCombo = await loginRepository.GetCombo(param);

                result = _mapper.Map<IEnumerable<Combo>>(entitieCombo);
            }
            catch (Exception ex)  {
                throw ex;
            }
            return result;
        }

        private bool AutenticarUsAd(string path, string dominioUsuario, string password, string usuario)
        {
            string user = "";
            try
            {
                bool isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
                if (isDevelopment)
                {
                    user = usuario;
                }
                else
                {
                    user = dominioUsuario;
                }
                DirectoryEntry ldapConnection = new DirectoryEntry(path, user, password);
                //ldapConnection.Path = ldap;
                ldapConnection.AuthenticationType = AuthenticationTypes.Secure;

                DirectorySearcher search = new DirectorySearcher(ldapConnection);

                search.Filter = "samAccountName=" + usuario;

                // Extraemos la primera coincidencia
                SearchResult oResult;
                oResult = search.FindOne();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
