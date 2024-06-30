using cattoapi.ClientModles;
using cattoapi.CustomResponse;
using cattoapi.Interfaces;
using cattoapi.Models;
using cattoapi.utlities;

namespace cattoapi.Repos
{
    public class AdminOperationsRepo : IAdminOperationsRepo
    {
        private readonly CattoDbContext _context;
        private readonly PasswordService _passwordService;

        public AdminOperationsRepo(CattoDbContext context, PasswordService passwordService) {
            _context = context;
            this._passwordService = passwordService;
        }



        public CustomResponse<bool> ChangeEmail(AdminChangeModel adminChangeModel)
        {

            if (adminChangeModel == null || !Utlities.IsValidEmail(adminChangeModel.email)
               || !Utlities.IsValidEmail(adminChangeModel.probertyChange))
                return new CustomResponse<bool>(400, "Email is not valid.");

            Account account = _context.Accounts.SingleOrDefault(acc => acc.Email.ToLower() == adminChangeModel.email.ToLower());

            if (account == null)
                return new CustomResponse<bool>(404, "The entered email is not assosciated wih any account.");


            Account emailExists = _context.Accounts.SingleOrDefault(acc => acc.Email.ToLower() == adminChangeModel.probertyChange.ToLower());

            if (emailExists != null)
                return new CustomResponse<bool>(409, "the email address is already associated with another account.");

            account.Email = adminChangeModel.probertyChange;
            try
            {
                _context.SaveChanges();
                return new CustomResponse<bool>(200, "Email changed successfully.");
            }
            catch
            {
                return new CustomResponse<bool>(500, "Something went wrong. Try again later");
            }


        }

        public CustomResponse<bool> ChangePassword(AdminChangeModel adminChangeModel)
        {
            if (adminChangeModel == null || !Utlities.IsValidEmail(adminChangeModel.email)
               || adminChangeModel.probertyChange == "")
                return new CustomResponse<bool>(400, "Email or password are not valid.");



            Account account = _context.Accounts.SingleOrDefault(acc => acc.Email.ToLower() == adminChangeModel.email.ToLower());

            if (account == null)
                 return new CustomResponse<bool>(404, "The entered email is not assosciated wih any account.");


            account.Password = _passwordService.HashPassword(account, adminChangeModel.probertyChange);
            try
            {
                _context.SaveChanges();
                return new CustomResponse<bool>(200, "Password changed successfully.");
            }
            catch
            {
                return new CustomResponse<bool>(500, "Something went wrong. Try again later");
            }

        }

        public CustomResponse<bool> ChangeRole(AdminChangeModel adminChangeModel)
        {
            if (adminChangeModel == null || !Utlities.IsValidEmail(adminChangeModel.email)
             || adminChangeModel.probertyChange == "")
                return new CustomResponse<bool>(400, "Email or role are not valid.");

            Account account = _context.Accounts.SingleOrDefault(acc => acc.Email.ToLower() == adminChangeModel.email.ToLower());

            if (account == null)
                return new CustomResponse<bool>(404, "The entered email is not assosciated wih any account.");

            if(account.Role == adminChangeModel.probertyChange)
                return new CustomResponse<bool>(409, "Role is already set to " + adminChangeModel.probertyChange.ToString());



            account.Role = adminChangeModel.probertyChange;
            try
            {
                _context.SaveChanges();
                return new CustomResponse<bool>(200, "Role changed successfully.");
            }
            catch
            {
                return new CustomResponse<bool>(500, "Something went wrong. Try again later");
            }
        }

        public CustomResponse<bool> ChangeUserName(AdminChangeModel adminChangeModel)
        {
            if (adminChangeModel == null || !Utlities.IsValidEmail(adminChangeModel.email)
            || adminChangeModel.probertyChange == "")
                return new CustomResponse<bool>(400, "Email or username are not valid.");


            Account account = _context.Accounts.SingleOrDefault(acc => acc.Email.ToLower() == adminChangeModel.email.ToLower());

            if (account == null)
                return new CustomResponse<bool>(404, "The entered email is not assosciated wih any account.");

            Account usernameExists = _context.Accounts.SingleOrDefault(acc => acc.UserName.ToLower() == adminChangeModel.probertyChange.ToLower());

            if (usernameExists != null)
                return new CustomResponse<bool>(409, "the username is already associated with another account.");

            account.UserName = adminChangeModel.probertyChange;
            
            try
            {
                _context.SaveChanges();
                return new CustomResponse<bool>(200, "Username changed successfully.");
            }
            catch
            {
                return new CustomResponse<bool>(500, "Something went wrong. Try again later");

            }
        }

        public CustomResponse<bool> DeleteAccount(string email)
        {
            if (email == null || !Utlities.IsValidEmail(email))
                return new CustomResponse<bool>(400, "Email is not valid.");


            Account account = _context.Accounts.SingleOrDefault(acc => acc.Email.ToLower() == email.ToLower());

            if (account == null)
                return new CustomResponse<bool>(404, "The entered email is not assosciated wih any account.");



            try
            {
                _context.Accounts.Remove(account);
                _context.SaveChanges();
                return new CustomResponse<bool>(200, "Account deleted successfully.");
            }
            catch
            {
                return new CustomResponse<bool>(500, "Something went wrong. Try again later");

            }
        }

        public CustomResponse<bool> RemovePFP(string email)
        {

            if (email == null || !Utlities.IsValidEmail(email))
                return new CustomResponse<bool>(400, "Email is not valid.");

            Account account = _context.Accounts.SingleOrDefault(acc => acc.Email.ToLower() == email.ToLower());

            if (account == null)
                return new CustomResponse<bool>(404, "The entered email is not assosciated wih any account.");

            if(account.Pfp == null)
                return new CustomResponse<bool>(409, "Profile picture is already null");


            account.Pfp = null;
            try
            {
                _context.SaveChanges();
                return new CustomResponse<bool>(200, "Profile picture deleted successfully.");
            }
            catch
            {
                return new CustomResponse<bool>(500, "Something went wrong. Try again later");

            }
        }

        public CustomResponse<bool> VerifyAccount(string email)
        {
            if (email == null || !Utlities.IsValidEmail(email))
                return new CustomResponse<bool>(400, "Email is not valid.");


            Account account = _context.Accounts.SingleOrDefault(acc => acc.Email.ToLower() == email.ToLower());

            if (account == null)
                return new CustomResponse<bool>(404, "The entered email is not assosciated wih any account.");

            if(account.Verified == true)
                return new CustomResponse<bool>(409, "the account is already verfied.");



            account.Verified = true;
            try
            {
                _context.SaveChanges();
                return new CustomResponse<bool>(200, "Account verifed successfully.");
            }
            catch
            {
                return new CustomResponse<bool>(500, "Something went wrong. Try again later");

            }
        }
    }



}
