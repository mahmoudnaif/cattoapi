using static cattoapi.utlities.Utlities;

namespace cattoapi.Interfaces.BlackListTokens
{
    public interface IBlackListTokensRepo
    {
        public Task BlacklistTokensAsync(int accountId, DateTime timestamp, TokenType tokenType);
        public Task<bool> IsTokenBlacklisted(int accountId, DateTime issuedAt);
    }
}
