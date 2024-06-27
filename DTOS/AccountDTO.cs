namespace cattoapi.DTOS
{
    public class AccountDTO
    {
        public long AccountId { get; set; }

        public string Email { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public byte[]? Pfp { get; set; }

        public string Role { get; set; } = null!;

        public bool Verified { get; set; }
    }
}
