using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VidaCamara.Domain.Services.Entidades.Apeseg;

namespace VidaCamara.Masivos.Services.Services.Apeseg
{
    public interface IApesegService
    {
        Task<RegistrarResponse> Apeseg_Registrar(RegistroSOATRequest request);
        Task<ModificarResponse> Apeseg_Actualizar(ModificarSOATRequest request);
        Task<AnularResponse> Apeseg_Anular(AnulacionSOATRequest request);
        Task<ConsultarResponse> Consultar(ConsultaSOATRequest request);
        Task<IEnumerable<string>> Apeseg_Validar(string tipo, dynamic request);
    }
}