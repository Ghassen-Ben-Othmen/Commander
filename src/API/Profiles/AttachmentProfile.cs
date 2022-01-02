using API.ViewModels.Attachment;
using AutoMapper;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Models.Utils.Utility;

namespace API.Profiles
{
    public class AttachmentProfile : Profile
    {
        public AttachmentProfile()
        {
            CreateMap<AttachmentCreate, Attachment>()
                .ForMember(dest => dest.Name, o => o.MapFrom(src => src.File.FileName))
                .ForMember(dest => dest.Size, o => o.MapFrom(src => src.File.Length));

            CreateMap<Attachment, AttachmentRead>();
        }
    }
}
