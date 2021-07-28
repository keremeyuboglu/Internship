using Altamira.Data.DTOs;
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
            CreateMap<UserUpdateDTO, User>();
        }
    }
}
