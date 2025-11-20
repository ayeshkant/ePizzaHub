using ePizzaHub.Application.Contracts;
using ePizzaHub.Application.DTOs.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ePizzaHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var response = await _itemService.GetAllItemsAsync();
            var commonResponse = new ApiResponseModelDto<IEnumerable<ItemResponseDto>>(true,"Data fetched", response);
            return Ok(commonResponse);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var response = await _itemService.GetItemByIdAsync(id);
            var commonResponse = new ApiResponseModelDto<ItemResponseDto>(true, "Data fetched", response);
            return Ok(commonResponse);
        }
    }
}
