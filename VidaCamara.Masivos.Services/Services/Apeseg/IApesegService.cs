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
        Task<(RegistrarResponse Resultado, ApesegLog Log)> Apeseg_Registrar(RegistroSOATRequest request);
        Task<(ModificarResponse Resultado, ApesegLog Log)> Apeseg_Actualizar(ModificarSOATRequest request);
        Task<(AnularResponse Resultado, ApesegLog Log)> Apeseg_Anular(AnulacionSOATRequest request);
        Task<(ConsultarResponse Resultado, ApesegLog Log)> Consultar(ConsultaSOATRequest request);
        Task<IEnumerable<string>> Apeseg_Validar(string tipo, dynamic request);
        Task Apeseg_Insertar(ApesegLog param);
    }
}