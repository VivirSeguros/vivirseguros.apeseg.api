using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using VivirSeguros.CRM.Application.Commons;
using VivirSeguros.CRM.Domain.Entities;
using VivirSeguros.CRM.Domain.Entities.Billing;
using VivirSeguros.CRM.Domain.Enums;
using VivirSeguros.CRM.Infrastructure.Commons;
using VivirSeguros.CRM.Infrastructure.Persistences.Interfaces;

namespace VivirSeguros.CRM.Application.Services.Billing.Commands.SendBillCommand
{
    public class SendBillHandler : IRequestHandler<SendBillCommand, Response<ResponseValidacion>>
    {

        private readonly IBillRepository _billRepository;
        private readonly IJobRepository _jobRepository;
        private readonly IHelperRepository _helperRepository;
        private readonly SendBillValidator _validationRules;
        private readonly IMapper _mapper;
        private readonly ILogger<SendBillHandler> _logger;

        public SendBillHandler(IBillRepository billRepository, IJobRepository jobRepository,
                               IHelperRepository helperRepository,
                               IMapper mapper, SendBillValidator validationRules,
                               ILogger<SendBillHandler> logger)
        {
            _billRepository = billRepository;
            _jobRepository = jobRepository;
            _helperRepository = helperRepository;
            _mapper = mapper;
            _validationRules = validationRules;
            _logger = logger;
        }

        public async Task<Response<ResponseValidacion>> Handle(SendBillCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<ResponseValidacion>();
            string fileName = "";
            RequestValidacion requestVal = new RequestValidacion();
            ResponseValidacion responseVal = new ResponseValidacion();
            Job job = new Job();
            BillFactBase infoBill = new BillFactBase();
            try
            {
                infoBill = await _billRepository.GetAsync(request.Skey);

                if (infoBill.boleta is not null && infoBill.factura is not null)
                {
                    response.IsSuccess = false;
                    response.Message = "Hubo un error al traer los datos del Skey" + request.Skey;
                } 
                else
                {
                    if (infoBill.idEstado == ((int)BillState.Terminado))
                    {
                        response.IsSuccess = false;
                        response.Message = "El " + request.Skey + " ya se encuentra procesado y la boleta fue aceptada por WS Facturador";
                    } 
                    else
                    {
                        job.ProductId = infoBill.idProducto;
                        job.TypeJobId = 1;
                        job.Skey = request.Skey;
                        job.State = ((int)BillState.Iniciado);
                        job.CreationUser = "WS";
                        job.DetailJob = new JobDetail();
                        //actualiza el trabajo como iniciado
                        await _jobRepository.UpdateAsync(job);

                        if (infoBill.tipoComprobante == "2")
                            fileName = infoBill.boleta.EMI.numeroDocId + "-" + infoBill.boleta.IDE.codTipoDocumento + "-" + infoBill.boleta.IDE.numeracion + ".json";
                        else
                            fileName = infoBill.factura.EMI.numeroDocId + "-" + infoBill.factura.IDE.codTipoDocumento + "-" + infoBill.factura.IDE.numeracion + ".json";

                        string pathFile = await _helperRepository.GetValorTablaConfig("PATFIL_FE");
                        string urlServiceFE = await _helperRepository.GetValorTablaConfig("USFE");
                        string pathFileName = pathFile + "\\" + fileName;

                        Util.WriteToJsonFile(pathFileName, infoBill, true);
                        requestVal.customer.username = await _helperRepository.GetValorTablaConfig("USRFE");
                        requestVal.customer.password = await _helperRepository.GetValorTablaConfig("PWDFE");
                        requestVal.fileName = fileName;
                        requestVal.fileContent = Util.ConvertToBase64(pathFileName);
                        responseVal = ApiService<ResponseValidacion, RequestValidacion>.PostRequest(urlServiceFE, requestVal);
                        //Util.DeleteFile(pathFileName);

                        if (responseVal.responseCode == "0")
                            job.State = ((int)BillState.Terminado);
                        else
                            job.State = ((int)BillState.ConError);

                        job.DetailJob.Request = Util.WriteToJson(infoBill);
                        job.DetailJob.Request1 = Util.WriteToJson(requestVal);
                        job.DetailJob.Response = responseVal.responseContent;
                        job.DetailJob.ResponseCode = responseVal.responseCode;
                        //actualiza el trabajo como finalizado y registra en el detalle los request y response correspondientes
                        await _jobRepository.UpdateAsync(job);

                        response.IsSuccess = true;
                        response.Message = "Envío correcto";
                        response.Data = responseVal;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }
    }

}
