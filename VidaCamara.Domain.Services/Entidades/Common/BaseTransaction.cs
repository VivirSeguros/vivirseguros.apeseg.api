using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidaCamara.Domain.Services.Entidades.Common
{
    public class BaseTransaction
    {
        public int Value { get; set; }
        public int Value1 { get; set; }
        public Int64 Value2{ get; set; }
        public int ErrorCode { get; set; }
        public string Message { get; set; }
    }
}
