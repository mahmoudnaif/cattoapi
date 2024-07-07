﻿using cattoapi.ClientModles;
using cattoapi.CustomResponse;
using cattoapi.Interfaces;
using cattoapi.Interfaces.BlackListTokens;
using cattoapi.Models;
using cattoapi.utlities;
using Microsoft.Identity.Client;
using static cattoapi.utlities.Utlities;

namespace cattoapi.Repos
{
    public class AdminOperationsRepo : IAdminOperationsRepo
    {
        private readonly CattoDbContext _context;
        private readonly PasswordService _passwordService;
        private readonly IBlackListTokensRepo _blackListTokensRepo;

        public AdminOperationsRepo(CattoDbContext context, PasswordService passwordService,IBlackListTokensRepo blackListTokensRepo) {
            _context = context;
            this._passwordService = passwordService;
            _blackListTokensRepo = blackListTokensRepo;
        }



        public CustomResponse<bool> ChangeEmail(AdminChangeModel adminChangeModel)
        {

            if (adminChangeModel == null || !Utlities.IsValidEmail(adminChangeModel.email)
               || !Utlities.IsValidEmail(adminChangeModel.probertyChange))
                return new CustomResponse<bool>(400, "Email is not valid.");

            Account account = _context.Accounts.SingleOrDefault(acc => acc.Email.ToLower() == adminChangeModel.email.ToLower());

            if (account == null)
                return new CustomResponse<bool>(404, "The entered email is not assosciated wih any account.");

            if (account.Role == "SUPPADUPPA")
                return new CustomResponse<bool>(401, "Can't change the email of this SUPPADUPPA account!");

            Account emailExists = _context.Accounts.SingleOrDefault(acc => acc.Email.ToLower() == adminChangeModel.probertyChange.ToLower());

            if (emailExists != null)
                return new CustomResponse<bool>(409, "the email address is already associated with another account.");


            account.Email = adminChangeModel.probertyChange;
            try
            {
                _context.SaveChanges();
                _blackListTokensRepo.BlacklistTokensAsync((int)account.AccountId, DateTime.UtcNow, TokenType.Login);
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

            if (!Utlities.IsValidPassword(adminChangeModel.probertyChange))
                return new CustomResponse<bool>(400, "Password must be at least 8 character long with at least 1 capital letter one small and a number.");

            Account account = _context.Accounts.SingleOrDefault(acc => acc.Email.ToLower() == adminChangeModel.email.ToLower());

            if (account == null)
                 return new CustomResponse<bool>(404, "The entered email is not assosciated wih any account.");

            if (account.Role == "SUPPADUPPA")
                return new CustomResponse<bool>(401, "Can't change the password of this SUPPADUPPA account!");

            account.Password = _passwordService.HashPassword(account, adminChangeModel.probertyChange);
            try
            {
                _context.SaveChanges();
                _blackListTokensRepo.BlacklistTokensAsync((int)account.AccountId, DateTime.UtcNow, TokenType.Login);
                return new CustomResponse<bool>(200, "Password changed successfully.");
            }
            catch
            {
                return new CustomResponse<bool>(500, "Something went wrong. Try again later");
            }

        }

        public CustomResponse<bool> ChangeRole(AdminChangeModel adminChangeModel)
        {
            if (adminChangeModel == null || !Utlities.IsValidEmail(adminChangeModel.email))
                return new CustomResponse<bool>(400, "Email id not valid.");


            if(adminChangeModel.probertyChange != "user" && adminChangeModel.probertyChange != "admin")
                return new CustomResponse<bool>(400, "Role is not valid.");



            Account account = _context.Accounts.SingleOrDefault(acc => acc.Email.ToLower() == adminChangeModel.email.ToLower());

            if (account == null)
                return new CustomResponse<bool>(404, "The entered email is not assosciated wih any account.");

            if(account.Role == adminChangeModel.probertyChange)
                return new CustomResponse<bool>(409, "Role is already set to " + adminChangeModel.probertyChange.ToString());


            if (account.Role == "SUPPADUPPA")
                return new CustomResponse<bool>(401, "Can't change the role of this SUPPADUPPA account!");



            account.Role = adminChangeModel.probertyChange;
            try
            {
                _context.SaveChanges();
                _blackListTokensRepo.BlacklistTokensAsync((int)account.AccountId, DateTime.UtcNow, TokenType.Login);
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

            if (!Utlities.IsValidUsername(adminChangeModel.probertyChange))
                return new CustomResponse<bool>(400, "Invalid username");

          


            Account account = _context.Accounts.SingleOrDefault(acc => acc.Email.ToLower() == adminChangeModel.email.ToLower());

            if (account == null)
                return new CustomResponse<bool>(404, "The entered email is not assosciated wih any account.");

            if (account.Role == "SUPPADUPPA")
                return new CustomResponse<bool>(401, "Can't change the username of this SUPPADUPPA account!");

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

            if (account.Role == "SUPPADUPPA")
                return new CustomResponse<bool>(401, "Can't delete this SUPPADUPPA account!");

            try
            {
                _context.Accounts.Remove(account);
                _context.SaveChanges();
                _blackListTokensRepo.BlacklistTokensAsync((int)account.AccountId, DateTime.UtcNow, TokenType.Login);
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

            if (account.Role == "SUPPADUPPA")
                return new CustomResponse<bool>(401, "Can't change the profile picture of this SUPPADUPPA account!");

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
