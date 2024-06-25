using cattoapi.ClientModles;

namespace cattoapi.Interfaces
{
    public interface ISiqiningOperationsRepo
    {
        public Task<bool> CreateAccountAsync(SiqnupModel siqnupModel);

    }
}
