using cattoapi.Models;

namespace cattoapi.Interfaces
{
    public interface IAccountRepo
    {
        public ICollection<Account> GetAccounts();

    }
}
