using cattoapi.Interfaces.BlackListTokens;
using Microsoft.Extensions.Caching.Memory;
using static cattoapi.utlities.Utlities;

namespace cattoapi.Repos.BlackListTokens
{
    public class BlackListTokensRepo : IBlackListTokensRepo
    {
        private readonly IMemoryCache _memoryCache;

        public BlackListTokensRepo(IMemoryCache memoryCache) {
            _memoryCache = memoryCache;
        }
        public Task BlacklistTokensAsync(int accountId, DateTime timestamp,TokenType tokenType)
        {
            int timeCached = 60; // defualt value
            switch (tokenType)
            {
                case TokenType.Login:
                    timeCached = 3*60;
                    break;

                case TokenType.EmailToken:
                    timeCached = 10;
                    break;

            }

            _memoryCache.Set(accountId, timestamp, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(timeCached)
            });
            return Task.CompletedTask;
        }

        public Task<bool> IsTokenBlacklisted(int accountId, DateTime issuedAt)
        {
            if (_memoryCache.TryGetValue(accountId, out DateTime blacklistTime))
            {
                return Task.FromResult(issuedAt <= blacklistTime);
            }
            return Task.FromResult(false);
        }
    }
}
