using System;
using System.Collections.Generic;

namespace cattoapi.Models;

public partial class Message
{
    public long Participant1Id { get; set; }

    public long Participant2Id { get; set; }

    public long SenderId { get; set; }

    public string Content { get; set; } = null!;

    public DateTime? Timestamp { get; set; }

    public bool Pending { get; set; }

    public long MessageId { get; set; }

    public virtual Conversation Conversation { get; set; } = null!;

    public virtual Account Sender { get; set; } = null!;
}
