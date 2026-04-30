using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using VidaCamara.Domain.Services.Entidades.Apeseg;

namespace VidaCamara.Domain.Services.Repositorios.Apeseg
{
    public interface IApesegRepository
    {
        Task<RegistrarResponse> Apeseg_Registrar(RegistrarParam param);
        Task<ModificarResponse> Apeseg_Actualizar(ModificarParam param);
        Task<AnularResponse> Apeseg_Anular(AnularParam param);
        Task<ConsultarResponse> Consultar(ConsultarParam param);
    }
}