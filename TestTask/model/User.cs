using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;

namespace TestTask.model
{
    internal class User
    {
        [Key]
        public Guid UserId { get; set; }

        [Required]
        public string Name { get; set; } = default!;

        [Required]
        public string Domain { get; set; } = default!;
        public List<Tag>? Tags { get; set; } = new List<Tag>();
    }
}
