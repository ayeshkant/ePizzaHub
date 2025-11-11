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
        public async Task<ActionResult<IEnumerable<ItemResponseDto>>> Get()
        {
            var response = await _itemService.GetAllItemsAsync();
            return Ok(response);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<ItemResponseDto>>> GetById(int id)
        {
            var response = await _itemService.GetItemByIdAsync(id);
            return Ok(response);
        }
    }
}
