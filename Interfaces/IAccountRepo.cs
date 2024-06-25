using cattoapi.ClientModles;
using cattoapi.Models;

namespace cattoapi.Interfaces
{
    public interface IAccountRepo
    {
        public ICollection<Account>? GetAccounts();
        public Account? GetAccountById(int id);

        public Account? GetAccountByEmail(string email);

        public ICollection<Account>? SearchAccounts(string searchQuery, int skip, int take)
            ;
        public  Task<bool> CreateAccountAsync(SiqnupModel signupModel);

    }
}
