using cattoapi.ClientModles;
using cattoapi.Interfaces;
using cattoapi.Models;
using cattoapi.utlities;
using Microsoft.EntityFrameworkCore;

namespace cattoapi.Repos
{
    public class Accountrepo : IAccountRepo
    {
        private readonly CattoDbContext _context;
        private readonly PasswordService _passwordService;

        public Accountrepo(CattoDbContext context, PasswordService passwordService)
        {
            _context = context;
            _passwordService = passwordService;
        }
        public ICollection<Account>? GetAccounts()
        {
            return _context.Accounts.ToList();
        }

        public Account? GetAccountByEmail(string email)
        {
            return _context.Accounts.SingleOrDefault(acc => acc.Email == email);
        }

        public Account? GetAccountById(int id)
        {
            return _context.Accounts.SingleOrDefault(acc => acc.AccountId == id);

        }


        public ICollection<Account>? SearchAccounts(string searchQuery,int take, int skip)
        {

            

            var searchResult = _context.Accounts.Where(acc => acc.UserName.StartsWith(searchQuery)).Skip(skip).Take(take).ToList();

            if(searchResult.Count == 0)
                return null;

            return searchResult;
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
