using AutoMapper;
using ePizzaHub.Application.DTOs.Response;
using ePizzaHub.Domain.Entities;
using ePizzaHub.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Application.Mappers
{
    public class ItemServiceMappingExtension:Profile
    {
        public ItemServiceMappingExtension()
        {
            CreateMap<ItemDomain, ItemResponseDto>();
        }
    }
}
