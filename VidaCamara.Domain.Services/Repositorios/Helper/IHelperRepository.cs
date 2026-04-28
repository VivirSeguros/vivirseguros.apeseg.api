using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VidaCamara.Domain.Services.Entidades;
using VidaCamara.Domain.Services.Entidades.Helper;

namespace VidaCamara.Domain.Services.Repositorios.Helper
{
    public interface IHelperRepository
    {
        Task<IEnumerable<Combo>> GetCombo(int comboId, int ventaDigital, int filtro, int filtro1);
        Task<Correo> GetCorreoInfo(int autoId);
        Task<ResponseTrx> GuardarDocumentoLog(LogTransacParam request);
        Task<string> GetValorTablaConfig(string skey);
        Task<string> GetUrlDocumentoSepelioCampofeAsync(int idCampofeTrama);       
        Task<string> GetTableConfigAsync(string skey, int productId);
    
    }
}
