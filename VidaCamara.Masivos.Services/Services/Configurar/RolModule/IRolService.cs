using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VidaCamara.Domain.Services.Entidades.Configurar;

namespace VidaCamara.Masivos.Services.Services.Configurar.RolModule
{
    public interface IRolService
    {
        Task<IEnumerable<Rol>> GetRol(int param);
        Task<Rol> InsRol(Rol param);


    }
}
