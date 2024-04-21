using AutoMapper;
using CustomerAPI.Dtos;
using DataBase.Models;

namespace CustomerAPI.Mapper
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<OnboardCustomerDto, Customer>();

            CreateMap<CreateLoginDetails, UserLogin>();
        }
    }
}
