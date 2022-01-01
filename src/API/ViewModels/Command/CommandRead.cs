using API.ViewModels.Argument;
using API.ViewModels.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels.Command
{
    public class CommandRead
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Cmd { get; set; }
        public long PlatformId { get; set; }
        public PlatformRead Platform { get; set; }
        public List<ArgumentRead> Arguments { get; set; }
    }
}
