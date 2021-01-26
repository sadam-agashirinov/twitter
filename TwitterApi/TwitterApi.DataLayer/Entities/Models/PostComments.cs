using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TwitterApi.DataLayer.Entities.Models
{
    public partial class PostComments
    {
        public PostComments()
        {
            CommentLikes = new HashSet<CommentLikes>();
        }

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
        public Guid ParentId { get; set; }
        public string Comment { get; set; }

        public virtual Posts Post { get; set; }
        public virtual Users User { get; set; }
        public virtual ICollection<CommentLikes> CommentLikes { get; set; }
    }
}
