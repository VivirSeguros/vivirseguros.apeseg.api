using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VivirSeguros.CRM.Application.Commons;
using VivirSeguros.CRM.Application.Services.Jobs.Commands.UpdateJobCommand;

namespace VivirSeguros.CRM.Application.Services.Endoso.Queries
{
    public class EndosoOrdinarioCommand : IRequest<ResponseEndoso<bool>>
    {
        public string NroDocumento { get; set; }
        public string celular { get; set; }
        public string celular1 { get; set; }
        public string celular2 { get; set; }
        public string Email { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public string Pais { get; set; }
        public string Departamento { get; set; }
        public string Provincia { get; set; }
        public string Distrito { get; set; }
        public string Direccion { get; set; }
    }
}
