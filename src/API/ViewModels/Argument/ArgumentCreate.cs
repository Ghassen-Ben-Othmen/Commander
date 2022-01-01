using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels.Argument
{
    public class ArgumentCreate
    {
        [Required]
        public string Value { get; set; }
        public string Description { get; set; }
        [Required]
        public long CommandId { get; set; }
    }
}
