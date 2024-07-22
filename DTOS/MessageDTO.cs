namespace cattoapi.DTOS
{
    public class MessageDTO
    {
        public long SenderId { get; set; }

        public string Content { get; set; } = null!;

        public DateTime? Timestamp { get; set; }

        public long MessageId { get; set; }
    }
}
