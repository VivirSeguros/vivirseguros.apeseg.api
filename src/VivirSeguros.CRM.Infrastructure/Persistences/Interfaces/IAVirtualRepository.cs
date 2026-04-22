using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VivirSeguros.CRM.Domain.Common;
using VivirSeguros.CRM.Domain.Entities.Producto;
using VivirSeguros.CRM.Domain.Entities.SAC;
using VivirSeguros.CRM.Domain.Entities.SAC.FONDOSMAX;
using VivirSeguros.CRM.Domain.Entities.SAC.RENTAMAX;
using VivirSeguros.CRM.Domain.Entities.SAC.RENTAVITALICIA;
using VivirSeguros.CRM.Domain.Entities.SAC.SOAT;
using VivirSeguros.CRM.Domain.Entities.SAC.VIVEMAX;

namespace VivirSeguros.CRM.Infrastructure.Persistences.Interfaces
{
    public interface IAVirtualRepository 
    {
        Task<IEnumerable<Producto>> GetProducto(string nroDoc);
        Task<ResponseTransaction> EndosoOrdinarioAsync(EndosoOrdinario endosoOrdinario);
        Task<ResponseTransaction> EndosoOrdinario2Async(EndosoOrdinario endosoOrdinario);
        Task<ResponseTransaction> EndosoOrdinario3Async(EndosoOrdinario endosoOrdinario);
        Task<ResponseTransaction> EndosoOrdinario4Async(EndosoOrdinario endosoOrdinario);
        Task<IEnumerable<Cliente>> GetCliente(string NroPoliza, string NroDocumento, string Placa, string Nombre1, string ApellidoPaterno);
        Task<IEnumerable<Polizasoat>> GetPolizaSoat(string NroPoliza, string NroDocumento);
        Task<IEnumerable<VehiculoSoat>> GetVehiculoSoat(string NroPoliza, string NroDocumento);
        Task<IEnumerable<ClienteSoat>> GetClienteSoat(string NroPoliza, string NroDocumento);
        Task<IEnumerable<ClienteVivemax>> GetClienteVivemax(string NroPoliza, string NroDocumento);
        Task<IEnumerable<ProductoVivemax>> GetProductoVivemax(string NroPoliza, string NroDocumento);
        Task<IEnumerable<PagoVivemax>> GetPagoVivemax(string NroPoliza, string NroDocumento);
        Task<IEnumerable<ClienteFondosmax>> GetClienteFondosmax(string NroPoliza, string NroDocumento);
        Task<IEnumerable<BeneficiarioFondosmax>> GetBeneficiarioFondosmax(string NroPoliza, string NroDocumento);
        Task<IEnumerable<ProductoFondosmax>> GetProductoFondosmax(string NroPoliza, string NroDocumento);
        Task<IEnumerable<PagoFondosmax>> GetPagoFondosmax(string NroPoliza, string NroDocumento);
        Task<IEnumerable<TitularRentamax>> GetTitularRentamax(string NroPoliza, string NroDocumento);
        Task<IEnumerable<AseguradoRentamax>> GetAseguradoRentamax(string NroPoliza, string NroDocumento);
        Task<IEnumerable<BeneficiarioRentamax>> GetBeneficiarioRentamax(string NroPoliza, string NroDocumento);
        Task<IEnumerable<ProductoRentamax>> GetProductoRentamax(string NroPoliza, string NroDocumento);
        Task<IEnumerable<TitularRentaVitalicia>> GetTitularRentaVitalicia(string NroPoliza, string NroDocumento);
        Task<IEnumerable<ProductoRentaVitalicia>> GetProductoRentaVitalicia(string NroPoliza, string NroDocumento);
        Task<IEnumerable<PagoRentaVitalicia>> GetPagoRentaVitalicia(string NroPoliza, string NroDocumento);
        Task<IEnumerable<BeneficiarioRentaVitalicia>> GetBeneficiarioRentaVitalicia(string NroPoliza, string NroDocumento);

    }
}
