using cattoapi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;


namespace cattoapi.utlities
{
    public static class Utlities
    {
        public enum TokenType
        {
            Login,
            EmailToken,
            
        }
        public static bool IsValidEmail(string email)
        {
            string emailRegex = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

            return Regex.IsMatch(email, emailRegex);
        }
        public static bool IsValidPassword(string password)
        {
            string passwordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$";

            return Regex.IsMatch (password, passwordRegex);
        }

        public static bool IsValidUsername(string username)
        {
            string usernameRegex = @"^[a-zA-Z0-9]([_](?![_])|[a-zA-Z0-9]){3,16}[a-zA-Z0-9]$";

            return Regex.IsMatch(username, usernameRegex);
        }

        public static string generateLoginJWT(int id, string role,string key)
        {
            var claims = new Claim[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, id.ToString()),
            new Claim("role", role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
             new Claim(JwtRegisteredClaimNames.Iat,
              ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds().ToString(),
              ClaimValueTypes.Integer64)
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

        public static string generateVerficationJWT(int id, string key)
        {
            var claims = new Claim[]
            {
            new Claim("accountId", id.ToString()),
            new Claim("Verify", "true"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
             new Claim(JwtRegisteredClaimNames.Iat,
              ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds().ToString(),
              ClaimValueTypes.Integer64)
            };

            var keyInBytes = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(keyInBytes, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
           issuer: "https://localhost:7180",
           audience: "https://localhost:7180",
           claims: claims,
           expires: DateTime.Now.AddMinutes(10),
           signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static string generateChangePasswordJWT(int id, string key)
        {
            var claims = new Claim[]
            {
            new Claim("accountId", id.ToString()),
            new Claim("changepassword", "true"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
             new Claim(JwtRegisteredClaimNames.Iat,
              ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds().ToString(),
              ClaimValueTypes.Integer64)
            };

            var keyInBytes = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(keyInBytes, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
           issuer: "https://localhost:7180",
           audience: "https://localhost:7180",
           claims: claims,
           expires: DateTime.Now.AddMinutes(10),
           signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public static bool IsImage(IFormFile file)
        {

            string[] allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".bmp" };

            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension))
            {
                return false;
            }

            return true;
        }

        public static async Task<byte[]> ConvertToByteArray(IFormFile file)
        {
            if (file == null)
                return null;
            try
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);

                    byte[] fileBytes = memoryStream.ToArray();


                    return fileBytes;
                    
                }
            }
            catch (Exception ex)
            {
                return [];
            }
        }



        public static bool IsImage(string file)
        {
            try
            {
                byte[] imageBytes = Convert.FromBase64String(GetBase64Data(file));
                using (Image<Rgba32> image = Image.Load<Rgba32>(imageBytes))
                {
                    return true; // Successfully loaded as an image
                }
            }
            catch (FormatException)
            {
                // Invalid Base64 string
                return false;
            }
            catch (Exception)
            {
                // Handle other exceptions as needed
                return false;
            }
        }
        public static byte[] ConvertToByteArray(string file)
        {
            if (file == null)
                return null;

            return Convert.FromBase64String(file);
        }
        private static string GetBase64Data(string base64String)
        {
            // Remove data URI scheme if present (e.g., data:image/jpeg;base64,)
            int commaIndex = base64String.IndexOf(',');
            return commaIndex == -1 ? base64String : base64String.Substring(commaIndex + 1);
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
