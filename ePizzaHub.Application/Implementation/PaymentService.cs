using AutoMapper;
using ePizzaHub.Application.Contracts;
using ePizzaHub.Application.DTOs.Request;
using ePizzaHub.Domain.Entities;
using ePizzaHub.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Application.Implementation
{
    public class PaymentService : IPaymentService
    {
        private readonly IMapper _mapper;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderRepository _orderRepository;

        public PaymentService(IMapper mapper,IPaymentRepository paymentRepository, IOrderRepository orderRepository)
        {
            _mapper = mapper;
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
        }
        public async Task<string> CapturePaymentDetailsAsync(MakePaymentRequestDto requestDto)
        {
            var paymentDomain = _mapper.Map<PaymentDomain>(requestDto);
            if (requestDto.OrderRequest is not null)
            {
                var orderDomain = _mapper.Map<OrderDomain>(requestDto.OrderRequest);
                await _orderRepository.AddAsync(orderDomain);
            }
            await _paymentRepository.AddAsync(paymentDomain);
            await _paymentRepository.CommitAsync();

            return await Task.FromResult("Payment Completed");
        }
    }
}
