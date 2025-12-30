using ePizzaHub.Application.Contracts;
using ePizzaHub.Application.DTOs.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ePizzaHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MakePaymentRequestDto requestDto)
        {
            var response=await _paymentService.CapturePaymentDetailsAsync(requestDto);
            return Ok(response);
        }
    }
}
