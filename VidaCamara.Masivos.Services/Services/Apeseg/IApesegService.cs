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
        Task<Registrar> Apeseg_Send(ApesegParam param);
        Task<Consultar> Consultar(string param);
        Task<Consultar> Consultar(string placa, string subscriptionKey);

        //Task<Consultar> Pago_Save(Respuesta param);
        //Task<int> Orden_Pago(OrdenPago param);
        //Task<int> Validar_Pago(string Orden, decimal Monto);
        //Task<IEnumerable<OrdenPago>> GetOrden(OrdenPagoParam param);
        //Task<ConsultaMasivaResponse> InsertaProcesoConsultaPlacasMasivo(ConsultaMasivaRequest consultaMasivaRequest);
        //Task<ConsultaMasivaResponse> ConsultaPlacasMasivo(ConsultaMasivaRequest consultaMasivaRequest, List<PlacaAPESEG> Placas);
        //Task<List<ApesegConsultaPlacaMasivo>> ConsultarProcesosMasivos();
        //Task<List<ApesegConsultaPlacaMasivoDetalle>> ConsultarProcesoDetalle(ConsultaMasivaRequest request);
        //Task<ConsultaMasivaResponse> ActualizaErrorPlacasMasivo(ConsultaMasivaRequest consultaMasivaRequest);
        //Task<ConsultaMasivaResponse> ActualizaProcesoMasivo(ConsultaMasivaRequest request);
        //Task<EnvioCarritoResponse> InsertaEnvioCarrito(EnvioCarritoRequest request);
        //Task<EnvioCarritoResponse> ActualizaEnvioCarrito(IEnumerable<string> placas, int toque);
    }
}