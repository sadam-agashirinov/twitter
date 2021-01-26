using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TwitterApi.DataLayer.Entities.Models
{
    public partial class BanList
    {
        public Guid Id { get; set; }
        public Guid WhoId { get; set; }
        public Guid WhomId { get; set; }

        public virtual Users Who { get; set; }
        public virtual Users Whom { get; set; }
    }
}
