using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static Models.Utils.Utility;

namespace API.ViewModels.Attachment
{
    public class AttachmentRead
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal Size { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public AttachmentType Type { get; set; }
        public long CommandId { get; set; }
    }
}
