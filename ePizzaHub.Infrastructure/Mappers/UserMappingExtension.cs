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
    public class UserMappingExtension:Profile
    {
        public UserMappingExtension()
        {
            CreateMap<UserDomain, User>().ReverseMap();
        }
    }
}
