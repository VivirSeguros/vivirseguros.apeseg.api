using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivirSeguros.CRM.Domain.Common
{
    public class ResponseTransaction
    {
        public int? ErrorCode { get; set; }
        public string Message { get; set; }
        public int? ErrorCode2 { get; set; }
        public string Message2 { get; set; }
        public int? ErrorCode3 { get; set; }
        public string Message3 { get; set; }
        public int? ErrorCode4 { get; set; }
        public string Message4 { get; set; }

    }
}
