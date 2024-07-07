using cattoapi.ClientModles;
using cattoapi.CustomResponse;
using cattoapi.Interfaces;
using cattoapi.Models;
using cattoapi.utlities;
using Microsoft.EntityFrameworkCore;

namespace cattoapi.Repos
{
    public class AuthOperationsRepo : IAuthOperationsRepo
    {


        private readonly CattoDbContext _context;
        private readonly PasswordService _passwordService;
        private readonly IConfiguration _configuration;

        public AuthOperationsRepo(CattoDbContext context, PasswordService passwordService,IConfiguration configuration)
        {
            _context = context;
            _passwordService = passwordService;
            _configuration = configuration;
        }




        public async Task<CustomResponse<bool>> CreateAccountAsync(SiqnupModel siqnupModel)
        {
            if (!Utlities.IsValidEmail(siqnupModel.email))
                return new CustomResponse<bool>(400, "Invalid email");

            if (!Utlities.IsValidUsername(siqnupModel.userName))
                return new CustomResponse<bool>(400, "Invalid username");

            if (!Utlities.IsValidPassword(siqnupModel.password))
                return new CustomResponse<bool>(400, "Password must be at least 8 character long with at least 1 capital letter one small and a number.");


            if (siqnupModel.password != siqnupModel.repeatPassword)
                 return new CustomResponse<bool>(400, "Passwords does not match");



            var emailUser = _context.Accounts.SingleOrDefault(acc => acc.Email.ToLower() == siqnupModel.email.ToLower());
            if (emailUser != null)
                return new CustomResponse<bool>(409, "Email already exists");

            var userNameUser = _context.Accounts.SingleOrDefault(acc => acc.UserName.ToLower() == siqnupModel.userName.ToLower());
               if(userNameUser != null)
                return new CustomResponse<bool>(409, "UserName already exists");


            Account account = new Account();

            account.Email = siqnupModel.email;
            account.UserName = siqnupModel.userName;
            account.Password = _passwordService.HashPassword(account, siqnupModel.password);
            account.DateCreated = DateTime.UtcNow;
            account.Role = "user";


            try
            {
                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();
                return new CustomResponse<bool>(201, "Account Created Successfully");
            }
            catch
            {
                return new CustomResponse<bool>(500, "Something went wrong! Please try again later"); ;
            }



            






        }



        public CustomResponse<Object> Signin(Siqninmodel siqninmodel)
        {
            Account account = null;
            
            if (utlities.Utlities.IsValidEmail(siqninmodel.emailOrUserName)) { 
            account = _context.Accounts.SingleOrDefault(acc => acc.Email.ToLower() == siqninmodel.emailOrUserName.ToLower());
            }
            else
            {
                account = _context.Accounts.SingleOrDefault(acc => acc.UserName == siqninmodel.emailOrUserName);
            }

            if (account == null)
            {
                return new CustomResponse<Object>(404,"Account was not found");
            }

            if (!_passwordService.VerifyPassword(account, siqninmodel.password))
                return new CustomResponse<Object>(401, "Check your password");

            string JWTToken = Utlities.generateLoginJWT((int)account.AccountId, account.Role, _configuration["Jwt:Key"]);



            return new CustomResponse<Object>(201, "Logged in successfully", new {Token = JWTToken });



        }
    }
}
