using cattoapi.ClientModles;
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

        public AuthOperationsRepo(CattoDbContext context, PasswordService passwordService)
        {
            _context = context;
            _passwordService = passwordService;
        }




        public async Task<bool> CreateAccountAsync(SiqnupModel siqnupModel)
        {
            if (!utlities.Utlities.IsValidEmail(siqnupModel.email) ||
                siqnupModel.password != siqnupModel.repeatPassword)
                return false;



            var emailUser = _context.Accounts.SingleOrDefault(acc => acc.Email.ToLower() == siqnupModel.email.ToLower());
            var userNameUser = _context.Accounts.SingleOrDefault(acc => acc.UserName.ToLower() == siqnupModel.userName.ToLower());

            if (emailUser != null || userNameUser != null)
                return false;


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
            }
            catch
            {
                return false;
            }



            return true;






        }



        public Account Signin(Siqninmodel siqninmodel)
        {
            Account account = null;
            
            if (utlities.Utlities.IsValidEmail(siqninmodel.emailOrUserName)) { 
            account = _context.Accounts.SingleOrDefault(acc => acc.Email.ToLower() == siqninmodel.emailOrUserName.ToLower());
            }
            else
            {
                account = _context.Accounts.SingleOrDefault(acc => acc.UserName == siqninmodel.emailOrUserName);
            }

            if (account == null || !_passwordService.VerifyPassword(account, siqninmodel.password))
            {
                return null;
            }

            return account;



        }
    }
}
