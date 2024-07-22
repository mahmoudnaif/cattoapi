using System;
using System.Collections.Generic;

namespace cattoapi.Models;

public partial class Account
{
    public long AccountId { get; set; }

    public string Email { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public byte[] Password { get; set; } = null!;

    public byte[]? Pfp { get; set; }

    public DateTime DateCreated { get; set; }

    public string Role { get; set; } = null!;

    public bool Verified { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Conversation> ConversationParticipant1s { get; set; } = new List<Conversation>();

    public virtual ICollection<Conversation> ConversationParticipant2s { get; set; } = new List<Conversation>();

    public virtual ICollection<LikedPost> LikedPosts { get; set; } = new List<LikedPost>();

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
