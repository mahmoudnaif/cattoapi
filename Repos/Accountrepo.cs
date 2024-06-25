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

        public Accountrepo(CattoDbContext context)
        {
            _context = context;
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






     }
}
