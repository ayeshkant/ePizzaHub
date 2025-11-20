using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Application.DTOs.Response
{
    public class ApiResponseModelDto<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public ApiResponseModelDto(bool success, string message, T data)
        {
            IsSuccess = success;
            Message = message;
            Data = data;
        }
    }
}
