using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VidaCamara.Domain.Services.Entidades.Configurar;

namespace VidaCamara.Masivos.Services.Services.Configurar.TipoPerfilModule
{
    public interface ITipoPerfilService
    {
        Task<IEnumerable<TipoPerfil>> GetTipoPerfil(int param, int prd);
        Task<TipoPerfil> InsTipoPerfil(TipoPerfil param);
    }
}
