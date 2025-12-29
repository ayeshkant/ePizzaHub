using ePizzaHub.UI.Constants;
using ePizzaHub.UI.Models.Request;
using ePizzaHub.UI.Models.Response;
using ePizzaHub.UI.Models.ViewModels;
using ePizzaHub.UI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ePizzaHub.UI.Controllers
{
    [Route("Cart")]
    public class CartController : BaseController
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CartController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        Guid CartId
        {
            get
            {
                Guid id;
                string cartId = Request.Cookies[ApplicationConstants.cartId] ?? string.Empty;
                if (string.IsNullOrEmpty(cartId))
                {
                    id = Guid.NewGuid();
                    Response.Cookies.Append(ApplicationConstants.cartId, id.ToString(), new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddDays(10)
                    });
                }
                else
                {
                    id = Guid.Parse(cartId);
                }
                return id;
            }
        }
        public async Task<IActionResult> Index()
        {
            using var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.ePizzaApiClient);
            var response = await httpClient.GetFromJsonAsync<ApiResponseModelDto<CartResponseDto>>($"/api/Cart/get-cart-detail?cartId={CartId}");
            
            return View(response.Data);
        }
        [HttpGet("AddToCart/{itemId:int}/{unitPrice:decimal}/{quantity:int}")]
        public async Task<JsonResult> AddToCart(int itemId, decimal unitPrice, int quantity)
        {
            using var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.ePizzaApiClient);
            var cartRequest = new AddToCartRequestDto
            {
                CartId=CartId,
                ItemId = itemId,
                UnitPrice = unitPrice,
                Quantity = quantity
            };
            var addToCartResponse = await httpClient.PostAsJsonAsync($"/api/Cart/add-items", cartRequest);
            addToCartResponse.EnsureSuccessStatusCode();

            var itemCount = await GetItemCount(CartId);
            
            return Json(new { count = itemCount });
        }
        [HttpGet("GetCartCount")]
        public async Task<JsonResult> GetCartItemsCount()
        {
            var itemCount = await GetItemCount(CartId);

            if (itemCount != null) return Json(new { count = itemCount});
            return Json(new { count = 0 });
        }
        [NonAction]
        public async Task<int> GetItemCount(Guid cartId)
        {
            using var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.ePizzaApiClient);

            var itemCount = await httpClient.GetFromJsonAsync<ApiResponseModelDto<int>>($"/api/Cart/get-item-count?cartId={CartId}");

            if (itemCount != null) return itemCount.Data;
            return await Task.FromResult(0);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CheckOut()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CheckOut(AddressViewModel request)
        {
            if (ModelState.IsValid && CurrentUser!=null)
            {
                using var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.ePizzaApiClient);
                var cartItems = await httpClient
                    .GetFromJsonAsync<ApiResponseModelDto<CartResponseDto>>($"/api/Cart/get-cart-detail?cartId={CartId}");

                //update the cart table with user id
                var updateCartRequest = new
                {
                    CartId = CartId,
                    UserId = CurrentUser.UserId
                };
                var updateUserResponse = await httpClient.PutAsJsonAsync("api/Cart/update-cart-user", updateCartRequest);
                updateUserResponse.EnsureSuccessStatusCode();

                TempData.Set("Address",request);
                TempData.Set("CartDetails",cartItems.Data);

                return RedirectToAction("Index", "Payment");
            }
            return View(request);
        }
    }
}
