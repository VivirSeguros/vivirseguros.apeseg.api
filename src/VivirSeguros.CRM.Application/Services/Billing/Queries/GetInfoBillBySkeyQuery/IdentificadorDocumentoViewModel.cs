using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivirSeguros.CRM.Application.Services.Billing.Queries.GetInfoBillBySkeyQuery
{
    public class IdentificadorDocumentoViewModel
    {
        public string Numeration { get; set; }
        public string IssueDate { get; set; }
        public string DocumentTypeId { get; set; }
        public string CurrencyType { get; set; }
        public string ExpirationDate { get; set; }
    }
}
