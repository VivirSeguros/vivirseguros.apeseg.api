using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VidaCamara.Domain.Services.Entidades.Configurar;

namespace VidaCamara.Domain.Services.Repositorios.Configurar
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<Usuario>> GetUsuario(int param);
        Task<Usuario> InsUsuario(Usuario param);
        Task<bool> ValidateExisteUsuario(string userName);
    }
}
