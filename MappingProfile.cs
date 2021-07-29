using Altamira.Data.DTOs;
using Altamira.Data.DTOs.Post;
using Altamira.Data.DTOs.Update;
using Altamira.Data.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altamira
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // TODO something about updating is not right, returns null values for unupdated properties
            CreateMap<UserPostDTO, User>().ReverseMap();
            CreateMap<UserUpdateDTO, User>().ReverseMap();
            CreateMap<AddressPostDTO, Address>().ReverseMap();
            CreateMap<AddressUpdateDTO, Address>().ReverseMap();
            CreateMap<CoordinatePostDTO, Coordinate>().ReverseMap();
            CreateMap<CoordinateUpdateDTO, Coordinate>().ReverseMap();
            CreateMap<CompanyPostDTO, Company>().ReverseMap();
            CreateMap<CompanyUpdateDTO, Company>().ReverseMap();
        }
    }
}
