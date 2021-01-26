using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TwitterApi.DataLayer.Entities.Models
{
    public partial class Posts
    {
        public Posts()
        {
            PostComments = new HashSet<PostComments>();
            PostLikes = new HashSet<PostLikes>();
        }

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Post { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual Users User { get; set; }
        public virtual ICollection<PostComments> PostComments { get; set; }
        public virtual ICollection<PostLikes> PostLikes { get; set; }
    }
}
