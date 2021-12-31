using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels.Platform
{
    public class PlatformRead
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public string Title { get; set; }
    }
}
