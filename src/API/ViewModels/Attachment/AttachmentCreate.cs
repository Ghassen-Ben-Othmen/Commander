using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels.Attachment
{
    public class AttachmentCreate
    {
        [Required]
        public long? CommandId { get; set; }
        [Required]
        public IFormFile File { get; set; }
    }
}
