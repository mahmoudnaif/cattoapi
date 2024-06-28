namespace cattoapi.DTOS
{
    public class ProfileDTO
    {
        public long AccountId { get; set; }
        public string UserName { get; set; } = null!;
        public byte[]? Pfp { get; set; }


    }
}
