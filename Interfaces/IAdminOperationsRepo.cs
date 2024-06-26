namespace cattoapi.Interfaces
{
    public interface IAdminOperationsRepo
    {
        public bool ChangePassword(string idOrEmail,string newPassword);

        public bool ChangeEmail(string idOrEmail, string newEmail);

        public bool ChangeUserName(string idOrEmail, string newUserName);


        public bool ChangeRole(string idOrEmail, string Newrole);

        public bool DeleteAccount(string idOrEmail);


        public bool VerifyAccount(string idOrEmail);

        public bool RemovePFP(string idOrEmail);

    }
}
