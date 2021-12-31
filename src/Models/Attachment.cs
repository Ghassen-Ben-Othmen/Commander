using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Models.Utils.Utility;

namespace Models
{
    public class Attachment
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal Size { get; set; }
        public AttachmentType Type { get; set; }
        public long CommandId { get; set; }
        public Command Command { get; set; }
    }
}
