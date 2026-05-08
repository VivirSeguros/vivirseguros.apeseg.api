using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using VidaCamara.Domain.Services.Entidades.Apeseg;

namespace VidaCamara.Domain.Services.Repositorios.Apeseg
{
    public interface IApesegRepository
    {
        void Apeseg_Insertar(ApesegLog param);
        Task<IEnumerable<ApesegErrorCatalogo>> ApesegErrorCatalogo_Listar();
        Task<IEnumerable<string>> Apeseg_Validar(string tipo, dynamic req);
    }
}