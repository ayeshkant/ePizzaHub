using ePizzaHub.UI.Models.Response;
using ePizzaHub.UI.Models.ViewModels;
using ePizzaHub.UI.RazorPay;
using ePizzaHub.UI.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ePizzaHub.UI.Controllers
{
    public class PaymentController : BaseController
    {
        private readonly IRazorPayService _razorPayService;
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory _clientFactory;

        public PaymentController(IRazorPayService razorPayService,
            IConfiguration configuration,
            IHttpClientFactory clientFactory)
        {
            this._razorPayService = razorPayService;
            this.configuration = configuration;
            this._clientFactory = clientFactory;
        }
        public IActionResult Index()
        {
            PaymentViewModel viewModel = new PaymentViewModel();

            CartResponseDto cartDetails
                 = JsonConvert.DeserializeObject<CartResponseDto>(TempData.Peek("CartDetails").ToString()!)!;

            if (cartDetails != null)
            {
                viewModel.RazorPayKey = configuration["RazorPay:Key"]!;
                viewModel.Currency = "INR";
                viewModel.GrantTotal = cartDetails.GrantTotal;
                viewModel.Cart = cartDetails;
                viewModel.Receipt = Guid.NewGuid().ToString();

                viewModel.OrderId = _razorPayService.CreateOrder(cartDetails.GrantTotal * 100, "INR", viewModel.Receipt);
            }

            return View(viewModel);
        }
        public IActionResult Status(IFormCollection forms)
        {
            string paymentId = forms["rzp_paymentid"]!;
            string orderId = forms["rzp_orderid"]!;
            string signature = forms["rzp_signature"]!;
            string currency = forms["Currency"]!;
            string transactionId = forms["Receipt"]!;

            bool isSignatureVerified = _razorPayService.VerifySignature(signature,orderId,paymentId);

            if (isSignatureVerified)
            {
                //calling api to insert data into database

                Response.Cookies.Delete("CartId");
                TempData.Remove("Address");
                TempData.Remove("CartDetails");

                return RedirectToAction("Receipt");
            }

            return View();
        }
        public IActionResult Receipt()
        {
            return View();
        }
    }
}
