using API.ViewModels.Platform;
using AutoMapper;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Profiles
{
    public class PlatformProfile : Profile
    {
        public PlatformProfile()
        {
            CreateMap<Platform, PlatformRead>();
            CreateMap<PlatformCreate, Platform>();
            CreateMap<PlatformUpdate, Platform>();
        }
    }
}
