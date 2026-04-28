using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using VidaCamara.Domain.Services.Entidades.Apeseg;

namespace VidaCamara.Domain.Services.Repositorios.Apeseg
{
    public interface IApesegRepository
    {
        Task<Registrar> Apeseg_Send(ApesegParam param);
        Task<Consultar> Consultar(string placa, string subscriptionKey = null);
        Task<Consultar> Pago_Save(Respuesta param);
        Task<int> Orden_Pago(OrdenPago param);
        Task<int> Validar_Pago(string Orden, decimal Monto);
        Task<IEnumerable<OrdenPago>> GetOrden(OrdenPagoParam param); 
        Task<ConsultaMasivaResponse> InsertaProcesoConsultaPlacasMasivo(ConsultaMasivaRequest consultaMasivaRequest);
        Task<ConsultaMasivaResponse> InsertaPlacasMasivo(ConsultaMasivaRequest consultaMasivaRequest, DataTable PlacasLista);
        Task<List<ApesegConsultaPlacaMasivo>> ConsultarProcesosMasivos();
        Task<List<ApesegConsultaPlacaMasivoDetalle>> ConsultarProcesoDetalle(ConsultaMasivaRequest request);
        Task<ConfigAPESEG> ObtieneConfigAPESEG();
        Task<ConsultaMasivaResponse> ActualizaErrorPlacasMasivo(ConsultaMasivaRequest consultaMasivaRequest);
        Task<ConsultaMasivaResponse> ActualizaPlacasMasivo(ConsultaMasivaRequest consultaMasivaRequest, ApesegConsultaPlacaMasivoDetalle Placa);
        Task<ConsultaMasivaResponse> ActualizaProcesoMasivo(ConsultaMasivaRequest request);
        Task<EnvioCarritoResponse> InsertaEnvioCarrito(EnvioCarritoRequest request);
        Task<EnvioCarritoResponse> ActualizaEnvioCarrito(ActualizaCarritoRequest request);
    }
}