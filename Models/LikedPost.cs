using System;
using System.Collections.Generic;

namespace cattoapi.Models;

public partial class LikedPost
{
    public long AccountId { get; set; }

    public long PostId { get; set; }

    public DateTime DateCreated { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Post Post { get; set; } = null!;
}
