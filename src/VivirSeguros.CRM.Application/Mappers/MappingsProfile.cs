using AutoMapper;
using VivirSeguros.CRM.Application.Dtos;
using VivirSeguros.CRM.Application.Services.Billing.Queries.GetInfoBillBySkeyQuery;
using VivirSeguros.CRM.Application.Services.Cuenta.Queries;
using VivirSeguros.CRM.Application.Services.Endoso.Queries;
using VivirSeguros.CRM.Application.Services.Jobs.Commands.AddJobCommand;
using VivirSeguros.CRM.Application.Services.Jobs.Commands.UpdateJobCommand;
using VivirSeguros.CRM.Application.Services.Jobs.Queries.GetJobByTypeIdQuery;
using VivirSeguros.CRM.Application.Services.Producto.Queries;
using VivirSeguros.CRM.Application.Services.SAC.FONDOSMAX.Queries;
using VivirSeguros.CRM.Application.Services.SAC.Queries;
using VivirSeguros.CRM.Application.Services.SAC.RENTAMAX.Queries;
using VivirSeguros.CRM.Application.Services.SAC.RENTAVITALICIA.Queries;
using VivirSeguros.CRM.Application.Services.SAC.SOAT.Queries;
using VivirSeguros.CRM.Application.Services.SAC.VIVEMAX.Queries;
using VivirSeguros.CRM.Domain.Entities;
using VivirSeguros.CRM.Domain.Entities.Billing;
using VivirSeguros.CRM.Domain.Entities.Producto;
using VivirSeguros.CRM.Domain.Entities.SAC;
using VivirSeguros.CRM.Domain.Entities.SAC.FONDOSMAX;
using VivirSeguros.CRM.Domain.Entities.SAC.RENTAMAX;
using VivirSeguros.CRM.Domain.Entities.SAC.RENTAVITALICIA;
using VivirSeguros.CRM.Domain.Entities.SAC.SOAT;
using VivirSeguros.CRM.Domain.Entities.SAC.VIVEMAX;
using VivirSeguros.CRM.Domain.Entities.ValidaCuenta;

namespace VivirSeguros.CRM.Application.Mappers
{
    public class MappingsProfile : Profile
    {
        public MappingsProfile()
        {
            CreateMap<AccountDto, Account>().ReverseMap();

            CreateMap<AddJobCommand, Job>().ReverseMap()
                 .ForMember(destination => destination.CreationUser, source => source.MapFrom(src => src.CreationUser))
                 .ForMember(destination => destination.State, source => source.MapFrom(src => src.State))
                 .ForMember(destination => destination.TypeJobId, source => source.MapFrom(src => src.TypeJobId));
            CreateMap<UpdateJobCommand, Job>().ReverseMap();
            CreateMap<JobDetailViewModel, JobDetail>().ReverseMap();

            CreateMap<GetJobByTipeIdViewModel, Job>().ReverseMap();
      
            CreateMap<BoletaBaseViewModel, BoletaBase>().ReverseMap();
            CreateMap<IdentificadorDocumentoViewModel, IdentificadorDocumento>().ReverseMap()
                .ForMember(destination => destination.Numeration, source => source.MapFrom(src => src.numeracion))
                .ForMember(destination => destination.IssueDate, source => source.MapFrom(src => src.fechaEmision))
                .ForMember(destination => destination.DocumentTypeId, source => source.MapFrom(src => src.codTipoDocumento))
                .ForMember(destination => destination.CurrencyType, source => source.MapFrom(src => src.tipoMoneda))
                .ForMember(destination => destination.ExpirationDate, source => source.MapFrom(src => src.fechaVencimiento));
            CreateMap<GetInfoBillBySkeyViewModel, BillFactBase>().ReverseMap()
                .ForMember(destination => destination.ticket, source => source.MapFrom(src => src.boleta));

            CreateMap<GetProductoViewModel, Producto>().ReverseMap();

            CreateMap<CuentaViewModel, Cuenta>().ReverseMap();

            CreateMap<EndosoOrdinarioCommand, EndosoOrdinario>().ReverseMap();

            CreateMap<GetClienteViewModel, Cliente>().ReverseMap();

            CreateMap<GetPolizaSoatViewModel, Polizasoat>().ReverseMap();

            CreateMap<GetVehiculoSoatViewModel, VehiculoSoat>().ReverseMap();

            CreateMap<GetClienteSoatViewModel, ClienteSoat>().ReverseMap();

            CreateMap<GetClienteVivemaxViewModel, ClienteVivemax>().ReverseMap();

            CreateMap<GetProductoVivemaxViewModel, ProductoVivemax>().ReverseMap();

            CreateMap<GetPagoVivemaxViewModel, PagoVivemax>().ReverseMap();

            CreateMap<GetClienteFondosmaxViewModel, ClienteFondosmax>().ReverseMap();

            CreateMap<GetBeneficiarioFondosmaxViewModel, BeneficiarioFondosmax>().ReverseMap();

            CreateMap<GetProductoFondosmaxViewModel, ProductoFondosmax>().ReverseMap();

            CreateMap<GetPagoFondosmaxViewModel, PagoFondosmax>().ReverseMap();

            CreateMap<GetTitularRentamaxViewModel, TitularRentamax>().ReverseMap();

            CreateMap<GetProductoRentamaxViewModel, ProductoRentamax>().ReverseMap();

            CreateMap<GetBeneficiarioRentamaxViewModel, BeneficiarioRentamax>().ReverseMap();

            CreateMap<GetAseguradoRentamaxViewModel, AseguradoRentamax>().ReverseMap();

            CreateMap<GetTitularRentaVitaliciaViewModel, TitularRentaVitalicia>().ReverseMap();

            CreateMap<GetProductoRentaVitaliciaViewModel, ProductoRentaVitalicia>().ReverseMap();

            CreateMap<GetPagoRentaVitaliciaViewModel, PagoRentaVitalicia>().ReverseMap();

            CreateMap<GetBeneficiarioRentaVitaliciaViewModel, BeneficiarioRentaVitalicia>().ReverseMap();


        }
    }
}