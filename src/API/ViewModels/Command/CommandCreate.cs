using API.ViewModels.Argument;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels.Command
{
    public class CommandCreate
    {
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string Cmd { get; set; }
        [Required]
        public long PlatformId { get; set; }
        public List<ArgumentCreate> Arguments { get; set; }
    }
}
