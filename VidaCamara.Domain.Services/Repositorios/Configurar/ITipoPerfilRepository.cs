using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VidaCamara.Domain.Services.Entidades.Configurar;

namespace VidaCamara.Domain.Services.Repositorios.Configurar
{
    public interface ITipoPerfilRepository
    {
        Task<IEnumerable<TipoPerfil>> GetTipoPerfil(int param, int prd);
        Task<TipoPerfil> InsTipoPerfil(TipoPerfil param);
    }
}
