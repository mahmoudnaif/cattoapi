using cattoapi.Interfaces;
using cattoapi.Models;
using cattoapi.utlities;
using System.Security.Claims;

namespace cattoapi.Repos
{
    public class UserOperationsRepo : IUserOperationsRepo
    {
        private readonly CattoDbContext _context;
        private readonly PasswordService _passwordService;

        public UserOperationsRepo(CattoDbContext context,PasswordService passwordService)
        {
            this._context = context;
            _passwordService = passwordService;
        }


        public Account GetData(int accountId)
        {
            Account account = _context.Accounts.FirstOrDefault(acc => acc.AccountId == accountId);


            if (account == null)
                return null;

            return account;
        }
        public bool ChangePassword(int accountId,string newPassword)
        {
            Account account = _context.Accounts.SingleOrDefault(acc => acc.AccountId== accountId);

            if (account == null)
                return false;


            account.Password = _passwordService.HashPassword(account, newPassword);
            try
            {
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Changepfp(int accountId, IFormFile pfp)
        {
            throw new NotImplementedException();
        }

        
    }
}
