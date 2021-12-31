using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Command
    {
        [Key]
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string Cmd { get; set; }
        public long PlatformId { get; set; }
        public Platform Platform { get; set; }
        public ICollection<Argument> Arguments { get; set; }
        public ICollection<Attachment> Attachments { get; set; }
    }
}
