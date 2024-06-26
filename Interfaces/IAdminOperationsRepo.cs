namespace cattoapi.Interfaces
{
    public interface IAdminOperationsRepo
    {
        public bool ChangePassword(string email,string newPassword);

        public bool ChangeEmail(string email, string newEmail);

        public bool ChangeUserName(string email, string newUserName);


        public bool ChangeRole(string email, string Newrole);

        public bool DeleteAccount(string email);


        public bool VerifyAccount(string email);

        public bool RemovePFP(string email);

    }
}
