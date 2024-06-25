using System;
using System.Collections.Generic;

namespace cattoapi.Models;

public partial class Post
{
    public long PostId { get; set; }

    public string? PostText { get; set; }

    public byte[]? PostPictrue { get; set; }

    public long LikesCount { get; set; }

    public long CommentsCount { get; set; }

    public long AccountId { get; set; }

    public DateTime DateCreated { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<LikedPost> LikedPosts { get; set; } = new List<LikedPost>();
}
