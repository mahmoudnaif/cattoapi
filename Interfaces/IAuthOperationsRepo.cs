using cattoapi.ClientModles;
using cattoapi.CustomResponse;

namespace cattoapi.Interfaces
{
    public interface IAuthOperationsRepo
    {
        public Task<CustomResponse<bool>> CreateAccountAsync(SiqnupModel siqnupModel);

        public CustomResponse<Object> Signin(Siqninmodel siqninmodel);

    }
}
