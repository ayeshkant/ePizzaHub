using AutoMapper;
using ePizzaHub.Domain.Entities;
using ePizzaHub.Infrastructure.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Infrastructure.Mappers
{
    public class UserTokenMappingExtension : Profile
    {
        public UserTokenMappingExtension()
        {
            CreateMap<UserTokenDomain, UserToken>();
            CreateMap<UserToken, UserTokenDomain>();
        }
    }

    
}

