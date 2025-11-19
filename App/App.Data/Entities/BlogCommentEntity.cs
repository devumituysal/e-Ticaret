using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Entities
{
    public class BlogCommentEntity : BaseEntity
    {
        public int BlogId { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Comment { get; set; } = null!;
        public bool IsApproved { get; set; }

        // Navigation properties
        public BlogEntity Blog { get; set; } = null!;
    }
}
