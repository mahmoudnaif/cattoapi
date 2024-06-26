using cattoapi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;


namespace cattoapi.utlities
{
    public static class Utlities
    {

        public static bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

            return Regex.IsMatch(email, pattern);
        }

        public static string generateLoginJWT(int id, string role,string key)
        {
            var claims = new Claim[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, id.ToString()),
            new Claim("role", role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var keyInBytes = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(keyInBytes, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
           issuer: "https://localhost:7180",
           audience: "https://localhost:7180",
           claims: claims,
           expires: DateTime.Now.AddHours(3),
           signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }

    public class PasswordService
    {
        private readonly PasswordHasher<Account> _passwordHasher;

        public PasswordService()
        {
            _passwordHasher = new PasswordHasher<Account>();
        }

        public byte[] HashPassword(Account user, string password)
        {
            string hashedPassword = _passwordHasher.HashPassword(user, password);
            return Convert.FromBase64String(hashedPassword);
        }

        public bool VerifyPassword(Account user, string providedPassword)
        {
            string storedPasswordHash = Convert.ToBase64String(user.Password);
            var result = _passwordHasher.VerifyHashedPassword(user, storedPasswordHash, providedPassword);
            return result == PasswordVerificationResult.Success;
        }
    }









}
