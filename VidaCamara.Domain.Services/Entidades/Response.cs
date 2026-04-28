using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace VidaCamara.Domain.Services.Entidades
{
    public class Response<T>
    {
        public T Data { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public IEnumerable<ValidationFailure> Errors { get; set; }
        public int Value { get; set; }
    }
}
