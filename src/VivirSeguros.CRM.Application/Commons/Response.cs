using FluentValidation.Results;
using System.Collections.Generic;

namespace VivirSeguros.CRM.Application.Commons
{
    public class Response<T>
    {
        public T Data { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int? CodeResponse { get; set; }
        public IEnumerable<ValidationFailure> Errors { get; set; }
    }
}
