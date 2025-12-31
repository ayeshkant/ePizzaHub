using ePizzaHub.Application.Contracts;
using ePizzaHub.Application.DTOs.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace ePizzaHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;
        private readonly IMemoryCache _memoryCache;

        public ItemController(IItemService itemService, IMemoryCache memoryCache)
        {
            _itemService = itemService;
            _memoryCache = memoryCache;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemResponseDto>>> Get()
        {
            IEnumerable<ItemResponseDto> response;
            _memoryCache.TryGetValue("Items", out response);
            if (response==null)
            {
                response = await _itemService.GetAllItemsAsync();
                _memoryCache.Set("Items", response,TimeSpan.FromMinutes(10));
            }
            //var commonResponse = new ApiResponseModelDto<IEnumerable<ItemResponseDto>>(true,"Data fetched", response);
            return Ok(response);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemResponseDto>> GetById(int id)
        {
            var response = await _itemService.GetItemByIdAsync(id);
            //var commonResponse = new ApiResponseModelDto<ItemResponseDto>(true, "Data fetched", response);
            return Ok(response);
        }
    }
}
