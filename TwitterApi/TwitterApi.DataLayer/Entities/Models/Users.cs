using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TwitterApi.DataLayer.Entities.Models
{
    public partial class Users
    {
        public Users()
        {
            BanListWho = new HashSet<BanList>();
            BanListWhom = new HashSet<BanList>();
            CommentLikes = new HashSet<CommentLikes>();
            PostComments = new HashSet<PostComments>();
            PostLikes = new HashSet<PostLikes>();
            Posts = new HashSet<Posts>();
            Tokens = new HashSet<Tokens>();
        }

        public Guid Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public virtual ICollection<BanList> BanListWho { get; set; }
        public virtual ICollection<BanList> BanListWhom { get; set; }
        public virtual ICollection<CommentLikes> CommentLikes { get; set; }
        public virtual ICollection<PostComments> PostComments { get; set; }
        public virtual ICollection<PostLikes> PostLikes { get; set; }
        public virtual ICollection<Posts> Posts { get; set; }
        public virtual ICollection<Tokens> Tokens { get; set; }
    }
}
