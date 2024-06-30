using cattoapi.ClientModles;
using cattoapi.CustomResponse;
using cattoapi.DTOS;
using cattoapi.Models;

namespace cattoapi.Interfaces
{
    public interface IAccountRepo
    {
        public CustomResponse<IEnumerable<AccountDTO>> GetAccounts();
        public CustomResponse<AccountDTO> GetAccountById(string strId);

        public CustomResponse<AccountDTO> GetAccountByEmail(string email);

        public CustomResponse<IEnumerable<AccountDTO>> SearchAccounts(string searchQuery, int skip, int take)
            ;

    }
}
