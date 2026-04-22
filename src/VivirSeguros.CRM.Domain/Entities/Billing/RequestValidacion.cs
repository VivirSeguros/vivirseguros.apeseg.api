using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivirSeguros.CRM.Domain.Entities.Billing
{
    public class RequestValidacion
    {
        public CustomerValidacion customer { get; set; } = new CustomerValidacion();
        public string fileName { get; set; }
        public string fileContent { get; set; }
    }
}
