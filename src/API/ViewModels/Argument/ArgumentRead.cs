using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels.Argument
{
    public class ArgumentRead
    {
        public long Id { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public long CommandId { get; set; }
    }
}
