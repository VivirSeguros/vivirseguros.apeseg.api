using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VidaCamara.Domain.Services.Entidades.Helper;

namespace VidaCamara.Masivos.Services.Services.Helper
{
    public interface IMaestraService
    {
        Task<IEnumerable<Maestra>> TipoPersona_sel();
        Task<IEnumerable<Maestra>> GetTipoCanal_SEL();
        Task<IEnumerable<Maestra>> TipoDocumento_sel(MaestraParam idMaestra);
    }
}
