using AutoMapper;
using ePizzaHub.Domain.Entities;
using ePizzaHub.Domain.Interfaces;
using ePizzaHub.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Infrastructure.Repositories
{
    public class PaymentRepository : GenericRepository<PaymentDomain, PaymentDetail>, IPaymentRepository
    {
        public PaymentRepository(ePizzaHubDBContext dBContext, IMapper mapper) : base(dBContext, mapper)
        {
        }
    }
}
