using cattoapi.CustomResponse;
using System.Security;

namespace cattoapi.Interfaces.EmailServices
{
    public interface IEmailServicesRepo
    {
        public Task<CustomResponse<bool>> SendVerificationEmail(int accountId);
        public CustomResponse<bool> VerifyAccount(int accountId);
        public Task<CustomResponse<bool>> SendPasswordChangeEmail(string email);

        public CustomResponse<bool> ChangePassword(int accountId, string newPassword, string repeatNewPassword);
    }
}
