using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.UI.Models.Response
{
    public class ApiResponseModelDto<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public ApiResponseModelDto(bool issuccess, string message, T data)
        {
            IsSuccess = issuccess;
            Message = message;
            Data = data;
        }
    }
}
