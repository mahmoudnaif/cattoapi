namespace cattoapi.DTOS
{
    public class PostDTO
    {
        public long PostId { get; set; }

        public string? PostText { get; set; }

        public byte[]? PostPictrue { get; set; }

        public long LikesCount { get; set; }

        public long CommentsCount { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
