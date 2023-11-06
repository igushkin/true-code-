using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TestTask.model
{
    internal class Tag
    {
        [Key]
        public Guid TagId { get; set; }

        [Required]
        public string Value { get; set; } = default!;

        [Required]
        public string Domain { get; set; } = default!;

        [JsonIgnore]
        public List<User>? Users { get; set; }

        public override string? ToString()
        {
            return this.TagId.ToString() + " " + this.Value + " " + this.Domain;
        }
    }
}
