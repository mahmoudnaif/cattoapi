using AutoMapper;
using cattoapi.CustomResponse;
using cattoapi.DTOS;
using cattoapi.Interfaces;
using cattoapi.Interfaces.BlackListTokens;
using cattoapi.Models;
using cattoapi.utlities;
using System.Security.Claims;
using static cattoapi.utlities.Utlities;

namespace cattoapi.Repos
{
    public class UserOperationsRepo : IUserOperationsRepo
    {
        private readonly CattoDbContext _context;
        private readonly PasswordService _passwordService;
        private readonly IMapper _mapper;
        private readonly IBlackListTokensRepo _blackListTokensRepo;

        public UserOperationsRepo(CattoDbContext context,PasswordService passwordService,IMapper mapper,IBlackListTokensRepo blackListTokensRepo)
        {
            this._context = context;
            _passwordService = passwordService;
            _mapper = mapper;
            _blackListTokensRepo = blackListTokensRepo;
        }


        public CustomResponse<AccountDTO> GetData(int accountId)
        {

            Account account = _context.Accounts.FirstOrDefault(acc => acc.AccountId == accountId);


            if (account == null)
                return new CustomResponse<AccountDTO>(404, "wtf man how did that even happen... wrong Id in a JTW?! I'm afed"); 


            AccountDTO accountDTO = _mapper.Map<AccountDTO>(account);
            return new CustomResponse<AccountDTO>(200, "Account info retreived successfully", accountDTO);


        }
        public CustomResponse<bool> ChangePassword(int accountId,string oldPassword, string newPassword)
        {
            if (!Utlities.IsValidPassword(newPassword))
                return new CustomResponse<bool>(400, "Password must be at least 8 character long with at least 1 capital letter one small and a number.");


            Account account = _context.Accounts.SingleOrDefault(acc => acc.AccountId== accountId);

            if (account == null)
                return new CustomResponse<bool>(404, "Account was not found");

            if (!_passwordService.VerifyPassword(account, oldPassword))
                return new CustomResponse<bool>(401, "Old password was wrong");


            account.Password = _passwordService.HashPassword(account, newPassword);
            try
            {
                _context.SaveChanges();
                _blackListTokensRepo.BlacklistTokensAsync(accountId, DateTime.UtcNow, TokenType.Login);
                return new CustomResponse<bool>(200, "Password cahnged sucessfully");
            }
            catch
            {
                return new CustomResponse<bool>(500, "Something went wrong try again later");
            }
        }
        public async Task<CustomResponse<bool>> Changepfp(int accountId, string pfp)
        {
            bool isImage = Utlities.IsImage(pfp);

            if (!isImage)
                return new CustomResponse<bool>(400, "Filetype is not supported");

            Account account = _context.Accounts.SingleOrDefault(acc => acc.AccountId == accountId);

            if(account == null)
                return new CustomResponse<bool>(404, "Account was not found");

            try
            {
                account.Pfp =  Utlities.ConvertToByteArray(pfp);
                Console.WriteLine(account.Pfp);
                _context.SaveChanges();
                return new CustomResponse<bool>(200, "Profile picture was set successfully");
            }
            catch
            {
                return new CustomResponse<bool>(500, "Something went wrong try again later");
            }








        }

        public CustomResponse<ProfileDTO> GetProfileById(string strId)
        {
            Account account;
            if (!int.TryParse(strId, out int id))
            {
                account = _context.Accounts.SingleOrDefault(acc => acc.UserName == strId);
            }
            else
            {
                account = _context.Accounts.SingleOrDefault(acc => acc.AccountId == id);
            }

            if (account == null)
                return new CustomResponse<ProfileDTO>(404, "NOT FOUND");


            ProfileDTO accountDTO = _mapper.Map<ProfileDTO>(account);

            return new CustomResponse<ProfileDTO>(200, "Account found", accountDTO);
        }
    }
}
