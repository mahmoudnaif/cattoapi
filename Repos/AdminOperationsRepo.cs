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
            if (!utlities.utlities.IsValidEmail(email)) 
                return false;
            

            Account account = _context.Accounts.SingleOrDefault(acc => acc.Email.ToLower() == email.ToLower());

            if (account == null)
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
            if (!utlities.utlities.IsValidEmail(email))
                return false;


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
            if (!utlities.utlities.IsValidEmail(email))
                return false;


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
            if (!utlities.utlities.IsValidEmail(email))
                return false;


            Account account = _context.Accounts.SingleOrDefault(acc => acc.Email.ToLower() == email.ToLower());

            if (account == null)
                return false;

            Account usernameexists = _context.Accounts.SingleOrDefault(acc => acc.UserName.ToLower() == newUserName.ToLower());

            if (usernameexists != null)
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
            if (!utlities.utlities.IsValidEmail(email))
                return false;


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
            if (!utlities.utlities.IsValidEmail(email))
                return false;


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
            if (!utlities.utlities.IsValidEmail(email))
                return false;


            Account account = _context.Accounts.SingleOrDefault(acc => acc.Email.ToLower() == email.ToLower());

            if (account == null)
                return false;


            
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
