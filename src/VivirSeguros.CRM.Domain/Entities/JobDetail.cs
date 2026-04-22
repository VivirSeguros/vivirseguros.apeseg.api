using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivirSeguros.CRM.Domain.Entities
{
    public class JobDetail
    {
        public int JobDetailId { get; set; }
        public int JobId { get; set; }
        public string Request { get; set; }
        public string Request1 { get; set; }
        public string Response { get; set; }
        public string ResponseCode { get; set; }
        public string CreationUser { get; set; }
        public DateTime RegisterDate { get; set; }
    }
}
