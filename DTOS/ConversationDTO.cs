namespace cattoapi.DTOS
{
    public class ConversationDTO
    {
        public long Participant1Id { get; set; }

        public long Participant2Id { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
