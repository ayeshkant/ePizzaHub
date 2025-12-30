using AutoMapper;
using ePizzaHub.Domain.Entities;
using ePizzaHub.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Infrastructure.Mappers
{
    public class PaymentMappingExtension : Profile
    {

        public PaymentMappingExtension()
        {
            CreateMap<PaymentDomain, PaymentDetail>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PaymentId))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));


            CreateMap<OrderDomain, Order>()
                  .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.OrderId))
                    .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<OrderItemDomain, OrderItem>();
        }
    }
}
