using cattoapi.ClientModles;
using cattoapi.Interfaces;
using cattoapi.Models;
using cattoapi.utlities;
using Microsoft.EntityFrameworkCore;

namespace cattoapi.Repos
{
    public class SiqiningOperationsRepo : ISiqiningOperationsRepo
    {


        private readonly CattoDbContext _context;
        private readonly PasswordService _passwordService;

        public SiqiningOperationsRepo(CattoDbContext context, PasswordService passwordService)
        {
            _context = context;
            _passwordService = passwordService;
        }







        public async Task<bool> CreateAccountAsync(SiqnupModel siqnupModel)
        {
            if (!utils.IsValidEmail(siqnupModel.email) ||
                siqnupModel.password != siqnupModel.repeatPassword)
                return false;



            var emailUser = _context.Accounts.SingleOrDefault(acc => acc.Email == siqnupModel.email);
            var userNameUser = _context.Accounts.SingleOrDefault(acc => acc.UserName == siqnupModel.userName);

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

    }
}
