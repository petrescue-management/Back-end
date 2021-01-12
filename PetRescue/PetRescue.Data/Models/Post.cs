using System;
using System.Collections.Generic;

namespace PetRescue.Data.Models
{
    public partial class Post
    {
        public Guid PetId { get; set; }
        public string PostContent { get; set; }
        public string PostStatus { get; set; }
        public Guid? InsertedBy { get; set; }
        public DateTime? InsertedAt { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual Pet Pet { get; set; }
    }
}
