using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Platform
    {
        [Key]
        public long Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string Title { get; set; }
        public ICollection<Command> Commands { get; set; }
    }
}
