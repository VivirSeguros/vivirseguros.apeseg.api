using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VidaCamara.Domain.Services.Entidades;
using VidaCamara.Domain.Services.Entidades.Common;
using VidaCamara.Domain.Services.Entidades.Helper;

namespace VidaCamara.Masivos.Services.Services.Helper
{
    public interface IHelperService
    {
        Task<IEnumerable<Combo>> GetCombo(int comboId, int ventaDigitalId, int filtro, int filtro1);
        Task<string> GetValorTablaConfig(string skey);
        void GuardarDocumentoLog(int autoId, string descripcion, string error, string urlDocumento, int paso, int tipoLog);
    }
}
