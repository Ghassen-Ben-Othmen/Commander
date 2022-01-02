using API.ViewModels.Argument;
using AutoMapper;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Profiles
{
    public class ArgumentProfile : Profile
    {
        public ArgumentProfile()
        {
            CreateMap<Argument, ArgumentRead>();
            CreateMap<ArgumentCreate, Argument>();
            CreateMap<ArgumentUpdate, Argument>()
                .ForAllMembers(opts =>
                {
                    opts.Condition((src, dest, member) => member != null);
                });
        }
    }
}
