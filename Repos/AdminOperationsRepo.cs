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



        public bool ChangeEmail(string email, string newEmail)
        {
           
            

            Account account = _context.Accounts.SingleOrDefault(acc => acc.Email.ToLower() == email.ToLower());

            if (account == null)
                return false;


            Account emailExists = _context.Accounts.SingleOrDefault(acc => acc.Email.ToLower() == newEmail.ToLower());

            if (emailExists != null)
                return false;

            account.Email = newEmail;
            try
            {
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }


        }

        public bool ChangePassword(string email, string newPassword)
        {
           


            Account account = _context.Accounts.SingleOrDefault(acc => acc.Email.ToLower() == email.ToLower());

            if (account == null)
                return false;


            account.Password = _passwordService.HashPassword(account, newPassword);
            try
            {
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool ChangeRole(string email, string Newrole)
        {
           

            Account account = _context.Accounts.SingleOrDefault(acc => acc.Email.ToLower() == email.ToLower());

            if (account == null)
                return false;


            account.Role = Newrole;
            try
            {
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ChangeUserName(string email, string newUserName)
        {
           


            Account account = _context.Accounts.SingleOrDefault(acc => acc.Email.ToLower() == email.ToLower());

            if (account == null)
                return false;

            Account usernameExists = _context.Accounts.SingleOrDefault(acc => acc.UserName.ToLower() == newUserName.ToLower());

            if (usernameExists != null)
                return false;

            account.UserName = newUserName;
            
            try
            {
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteAccount(string email)
        {
           


            Account account = _context.Accounts.SingleOrDefault(acc => acc.Email.ToLower() == email.ToLower());

            if (account == null)
                return false;


            
            try
            {
                _context.Accounts.Remove(account);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool RemovePFP(string email)
        {
           


            Account account = _context.Accounts.SingleOrDefault(acc => acc.Email.ToLower() == email.ToLower());

            if (account == null)
                return false;


            account.Pfp = null;
            try
            {
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool VerifyAccount(string email)
        {

            Account account = _context.Accounts.SingleOrDefault(acc => acc.Email.ToLower() == email.ToLower());

            if (account == null)
                return false;

            account.Verified = true;

            
            try
            {
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }



}
