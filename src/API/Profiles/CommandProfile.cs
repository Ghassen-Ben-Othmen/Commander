using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using API.ViewModels.Command;

namespace API.Profiles
{
    public class CommandProfile : Profile
    {
        public CommandProfile()
        {
            CreateMap<Command, CommandRead>();
            CreateMap<CommandCreate, Command>();
            CreateMap<CommandUpdate, Command>().ForAllMembers(opts =>
            {
                opts.Condition((src, des, member) => member != null);
            });
        }
    }
}
