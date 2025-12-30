using ePizzaHub.Application.DTOs.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Application.Contracts
{
    public interface IPaymentService
    {
        Task<string> CapturePaymentDetailsAsync(MakePaymentRequestDto requestDto);
    }
}
