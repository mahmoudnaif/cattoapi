using cattoapi.customResponse;
using cattoapi.DTOS;
using cattoapi.Models;

namespace cattoapi.Interfaces
{
    public interface IUserOperationsRepo
    {
        public CustomResponse<AccountDTO> GetData(int accountId);

        public CustomResponse<bool> ChangePassword(int accountId, string oldPassword, string newPassword);

        public Task<CustomResponse<bool>> Changepfp(int accountId, string pfp);

        public CustomResponse<ProfileDTO> GetProfileById(string strId);

    }
}
