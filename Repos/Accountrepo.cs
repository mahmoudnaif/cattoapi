using cattoapi.Interfaces;
using cattoapi.Models;

namespace cattoapi.Repos
{
    public class Accountrepo : IAccountRepo
    {
        private readonly CattoDbContext _context;

        public Accountrepo(CattoDbContext context)
        {
            _context = context;
        }

        public ICollection<Account> GetAccounts()
        {
            return _context.Accounts.ToList();
        }

    }
}
