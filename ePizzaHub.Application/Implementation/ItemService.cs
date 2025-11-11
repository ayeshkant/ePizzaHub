using AutoMapper;
using ePizzaHub.Application.Contracts;
using ePizzaHub.Application.DTOs.Response;
using ePizzaHub.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Application.Implementation
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;

        public ItemService(IItemRepository itemRepository, IMapper mapper)
        {
            _itemRepository = itemRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<ItemResponseDto>> GetAllItemsAsync()
        {
            var response = await _itemRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ItemResponseDto>>(response);
        }
        public async Task<ItemResponseDto> GetItemByIdAsync(int id)
        {
            var response = await _itemRepository.GetItemByIdAsync(id);
            return _mapper.Map<ItemResponseDto>(response);
        }
    }
}
