using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TwitterApi.DataLayer.Entities.Models
{
    public partial class Tokens
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpireTime { get; set; }
        public bool Used { get; set; }

        public virtual Users User { get; set; }
    }
}
