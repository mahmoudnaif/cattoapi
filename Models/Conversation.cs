using System;
using System.Collections.Generic;

namespace cattoapi.Models;

public partial class Conversation
{
    public long Participant1Id { get; set; }

    public long Participant2Id { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual Account Participant1 { get; set; } = null!;

    public virtual Account Participant2 { get; set; } = null!;
}
