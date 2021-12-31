using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Argument
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string Value { get; set; }
        public string Description { get; set; }
        public long CommandId { get; set; }
        public Command Command { get; set; }
    }
}
