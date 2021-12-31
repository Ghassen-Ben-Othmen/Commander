using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Command
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Cmd { get; set; }
    }
}
