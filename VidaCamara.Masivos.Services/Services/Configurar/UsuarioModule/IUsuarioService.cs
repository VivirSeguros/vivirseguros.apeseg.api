using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VidaCamara.Domain.Services.Entidades.Configurar;

namespace VidaCamara.Masivos.Services.Services.Configurar.UsuarioModule
{
    public interface IUsuarioService    {        
        Task<IEnumerable<Usuario>> GetUsuario(int param);
        Task<Usuario> InsUsuario(Usuario param);
        Task<bool> ValidateExisteUsuario(string userName);
    }
}
