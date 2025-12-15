using AutoMapper;
using ePizzaHub.Application.DTOs.Request;
using ePizzaHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Application.Mappers
{
    public class UserServiceMappingExtension:Profile
    {
        public UserServiceMappingExtension()
        {
            CreateMap<RegisterUserDto, UserDomain>();
        }
    }
}
