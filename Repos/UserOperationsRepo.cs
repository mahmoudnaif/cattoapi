using AutoMapper;
using cattoapi.customResponse;
using cattoapi.DTOS;
using cattoapi.Interfaces;
using cattoapi.Models;
using cattoapi.utlities;
using System.Security.Claims;

namespace cattoapi.Repos
{
    public class UserOperationsRepo : IUserOperationsRepo
    {
        private readonly CattoDbContext _context;
        private readonly PasswordService _passwordService;
        private readonly IMapper _mapper;

        public UserOperationsRepo(CattoDbContext context,PasswordService passwordService,IMapper mapper)
        {
            this._context = context;
            _passwordService = passwordService;
            _mapper = mapper;
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
            Account account = _context.Accounts.SingleOrDefault(acc => acc.AccountId== accountId);

            if (account == null)
                return new CustomResponse<bool>(404, "Account was not found");

            if (!_passwordService.VerifyPassword(account, oldPassword))
                return new CustomResponse<bool>(401, "Old password was wrong");


            account.Password = _passwordService.HashPassword(account, newPassword);
            try
            {
                _context.SaveChanges();
                return new CustomResponse<bool>(200, "Password cahnged sucessfully");
            }
            catch
            {
                return new CustomResponse<bool>(500, "Something went wrong try again later");
            }
        }
        public async Task<CustomResponse<bool>> Changepfp(int accountId, IFormFile pfp)
        {
            bool isImage = Utlities.IsImage(pfp);

            if (!isImage)
                return new CustomResponse<bool>(400, "Filetype is not supported");

            Account account = _context.Accounts.SingleOrDefault(acc => acc.AccountId == accountId);

            if(account == null)
                return new CustomResponse<bool>(404, "Account was not found");

            try
            {
                account.Pfp = await Utlities.ConvertToByteArray(pfp);
                Console.WriteLine(account.Pfp);
                _context.SaveChanges();
                return new CustomResponse<bool>(200, "Profile picture was set successfully");
            }
            catch
            {
                return new CustomResponse<bool>(500, "Something went wrong try again later");
            }








        }


        
    }
}
