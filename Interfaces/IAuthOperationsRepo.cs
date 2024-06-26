using cattoapi.ClientModles;
using cattoapi.Models;

namespace cattoapi.Interfaces
{
    public interface IAuthOperationsRepo
    {
        public Task<bool> CreateAccountAsync(SiqnupModel siqnupModel);

        public Account Signin(Siqninmodel siqninmodel);

    }
}
