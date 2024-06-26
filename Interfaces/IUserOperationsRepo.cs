using cattoapi.Models;

namespace cattoapi.Interfaces
{
    public interface IUserOperationsRepo
    {
        public Account GetData(int accountId);

        public bool ChangePassword(int accountId, string oldPassword, string newPassword);

        public Task<bool> Changepfp(int accountId, IFormFile pfp);

    }
}
