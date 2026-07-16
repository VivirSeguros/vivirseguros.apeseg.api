using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VidaCamara.Domain.Services.Entidades.Configurar;

namespace VidaCamara.Domain.Services.Repositorios.Configurar
{
    public interface IRolRepository
    {
        Task<IEnumerable<Rol>> GetRol(int param);
        Task<Rol> InsRol(Rol param);


    }
}
