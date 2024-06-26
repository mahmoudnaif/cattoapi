using cattoapi.Models;

namespace cattoapi.Interfaces
{
    public interface IUserOperationsRepo
    {
        public Account GetData(int accountId);

        public bool ChangePassword(int accountId, string newPassword);

        public bool Changepfp(int accountId, IFormFile pfp);

    }
}
