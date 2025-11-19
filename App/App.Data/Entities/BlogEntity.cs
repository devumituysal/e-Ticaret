using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Entities
{
    public class BlogEntity : BaseEntity
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public int UserId { get; set; }
        public bool Enabled { get; set; } = true;

        // Navigation properties
        public UserEntity User { get; set; } = null!;

        public ICollection<BlogCommentEntity> Comments { get; set; } = null!;
    }
}
