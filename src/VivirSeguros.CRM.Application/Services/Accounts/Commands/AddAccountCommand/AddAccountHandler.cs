using AutoMapper;
using VivirSeguros.CRM.Application.Commons;
using VivirSeguros.CRM.Domain.Entities;
using VivirSeguros.CRM.Infrastructure.Persistences.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;
using VivirSeguros.CRM.Domain.Common;

namespace VivirSeguros.CRM.Application.Services.Accounts.Commands.AddAccountCommand
{
    public class AddAccountHandler : IRequestHandler<AddAccountCommand, Response<bool>>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;

        public AddAccountHandler(IAccountRepository accountRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;            
        }

        public async Task<Response<bool>> Handle(AddAccountCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<bool>();
            var result = new BaseTransaction();
            try
            {
                Account account = new Account { Username = request.Username, 
                                                Secret = BC.HashPassword(request.Secret) };
                //var account = _mapper.Map<Account>(request);
                result = await _accountRepository.InsertAsync(account);

                if (result.ErrorCode == 0)
                {
                    response.IsSuccess = true;
                    response.CodeResponse = result.ErrorCode;
                    response.Message = "Registro Exitoso!!!";
                }
                else if(result.ErrorCode == 1)
                {
                    response.IsSuccess = false;
                    response.CodeResponse = result.ErrorCode;
                    response.Message = "Usuario ya existe!!!";
                }  
                else
                {
                    response.IsSuccess = false;
                    response.CodeResponse = result.ErrorCode;
                    response.Message = "Registro Fallido!!!";
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
