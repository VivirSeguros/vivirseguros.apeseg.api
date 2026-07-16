using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VidaCamara.Domain.Services.Entidades.Helper;

namespace VidaCamara.Domain.Services.Repositorios.Helper
{
   public interface IMaestraRepository
    {
        Task<IEnumerable<Maestra>> TipoPersona_sel();
        Task<IEnumerable<Maestra>> GetTipoCanal_SEL();
        Task<IEnumerable<Maestra>> TipoDocumento_sel(MaestraParam idMaestra);
    }
}
