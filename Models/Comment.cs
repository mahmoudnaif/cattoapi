using System;
using System.Collections.Generic;

namespace cattoapi.Models;

public partial class Comment
{
    public long CommentId { get; set; }

    public string CommentText { get; set; } = null!;

    public long AccountId { get; set; }

    public long PostId { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Post Post { get; set; } = null!;
}
