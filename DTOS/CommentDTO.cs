namespace cattoapi.DTOS
{
    public class CommentDTO
    {
        public long CommentId { get; set; }

        public string CommentText { get; set; } = null!;

        public DateTime DateCreated { get; set; }

        public ProfileDTO? Profile { get; set; }

    }
}
