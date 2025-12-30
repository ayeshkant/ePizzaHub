using ePizzaHub.UI.Constants;
using ePizzaHub.UI.Models.Request;
using ePizzaHub.UI.Models.Response;
using ePizzaHub.UI.Models.ViewModels;
using ePizzaHub.UI.RazorPay;
using ePizzaHub.UI.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;

namespace ePizzaHub.UI.Controllers
{
    public class PaymentController : BaseController
    {
        private readonly IRazorPayService _razorPayService;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _clientFactory;

        public PaymentController(IRazorPayService razorPayService,
            IConfiguration configuration,
            IHttpClientFactory clientFactory)
        {
            _razorPayService = razorPayService;
            _configuration = configuration;
            _clientFactory = clientFactory;
        }
        public IActionResult Index()
        {
            PaymentViewModel viewModel = new PaymentViewModel();

            CartResponseDto cartDetails
                 = TempData.Get<CartResponseDto>("CartDetails");

            if (cartDetails != null)
            {
                viewModel.RazorPayKey = _configuration["RazorPay:Key"]!;
                viewModel.Currency = "INR";
                viewModel.GrantTotal = cartDetails.GrantTotal;
                viewModel.Cart = cartDetails;
                viewModel.Receipt = Guid.NewGuid().ToString();

                viewModel.OrderId = _razorPayService.CreateOrder(cartDetails.GrantTotal * 100, "INR", viewModel.Receipt);
            }

            return View(viewModel);
        }
        public async Task<IActionResult> Status(IFormCollection forms)
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
                var paymentDetails = _razorPayService.GetPayment(paymentId);
                string status = paymentDetails["status"];
                var request = GetPaymentRequest(paymentId, orderId, transactionId, currency, status);
                var jsonRequest = JsonConvert.SerializeObject(request);
                var client = _clientFactory.CreateClient(ApplicationConstants.ePizzaApiClient);
                var response = await client.PostAsJsonAsync("api/Payment", request);

                response.EnsureSuccessStatusCode();

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
        private MakePaymentRequestDto GetPaymentRequest(
         string paymentId, string orderid, string transactionid, string currency, string status)
        {
            CartResponseDto cart = TempData.Get<CartResponseDto>("CartDetails");

            AddressViewModel addressViewModel = TempData.Get<AddressViewModel>("Address");


            return new MakePaymentRequestDto
            {
                CartId = Guid.Parse(Request.Cookies["CartId"])!,
                Total = cart.Total,
                Currency = currency,
                PaymentId = paymentId,
                Status = status,
                TransactionId = transactionid,
                Tax = cart.Tax,
                Email = CurrentUser.Email,
                GrandTotal = cart.GrantTotal,
                UserId = CurrentUser.UserId,
                OrderRequest = new OrderRequestModelDto()
                {
                    City = addressViewModel.City,
                    Locality = addressViewModel.Locality,
                    Street = addressViewModel.Street,
                    UserId = CurrentUser.UserId,
                    OrderId = orderid,
                    PaymentId = paymentId,
                    PhoneNumber = addressViewModel.PhoneNumber,
                    ZipCode = addressViewModel.ZipCode,
                    OrderItems = GetOrderItems(cart.CartItems)
                }
            };
        }

        private List<OrderItemsRequestDto> GetOrderItems(List<CartItemsResponseDto> items)
        {
            List<OrderItemsRequestDto> orderItems = [];
            items.ForEach(x => orderItems.Add(new OrderItemsRequestDto()
            {
                ItemId = x.ItemId,
                Quantity = x.Quantity,
                Total = x.ItemTotal,
                UnitPrice = x.UnitPrice
            }));

            return orderItems;
        }
    }
}
