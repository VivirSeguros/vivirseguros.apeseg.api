using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivirSeguros.CRM.Application.Commons
{
    public class ResponseEndoso<T>
    {
        public T Data { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public bool IsSuccess2 { get; set; }
        public string Message2 { get; set; }
        public bool IsSuccess3 { get; set; }
        public string Message3 { get; set; }

        public bool IsSuccess4 { get; set; }
        public string Message4 { get; set; }

    }
}
