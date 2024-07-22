using cattoapi.Interfaces.BlackListTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace cattoapi.Repos
{
    public class TokensRepo
    {
        private readonly IConfiguration _configuration;
        private readonly IBlackListTokensRepo _blackListTokensRepo;

        public TokensRepo(IConfiguration configuration, IBlackListTokensRepo blackListTokensRepo)
        {
            _configuration = configuration;
            _blackListTokensRepo = blackListTokensRepo;
        }

        public async Task<int> IsTokenValid(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]))
                };

                SecurityToken validatedToken;
                ClaimsPrincipal principal = handler.ValidateToken(token, validationParameters, out validatedToken);


                int userId = int.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier));

                var iatClaim = principal.FindFirstValue(JwtRegisteredClaimNames.Iat);

                var issuedAt = DateTimeOffset.FromUnixTimeSeconds(long.Parse(iatClaim)).UtcDateTime;


                if (await _blackListTokensRepo.IsTokenBlacklisted(userId, issuedAt))
                {
                    throw new Exception();
                }

                return userId;
            }
            catch (Exception)
            {
                return 0;
            }
        }

    }
 }
