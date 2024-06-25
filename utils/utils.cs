using cattoapi.Models;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace cattoapi.utlities
{
    public static class utils
    {
        public static bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

            return Regex.IsMatch(email, pattern);
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
