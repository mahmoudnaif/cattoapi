using cattoapi.ClientModles;
using cattoapi.CustomResponse;

namespace cattoapi.Interfaces
{
    public interface IAdminOperationsRepo
    {
        public CustomResponse<bool> ChangePassword(AdminChangeModel adminChangeModel);

        public CustomResponse<bool> ChangeEmail(AdminChangeModel adminChangeModel);

        public CustomResponse<bool> ChangeUserName(AdminChangeModel adminChangeModel);


        public CustomResponse<bool> ChangeRole(AdminChangeModel adminChangeModel);

        public CustomResponse<bool> DeleteAccount(string email);


        public CustomResponse<bool> VerifyAccount(string email);

        public CustomResponse<bool> RemovePFP(string email);

    }
}
