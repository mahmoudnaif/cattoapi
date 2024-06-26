namespace cattoapi.ClientModles
{
    public class SearchModel
    {
       public string searchQuery { get; set; }
        public int take { get; set; } = 20;
        public int skip { get; set; } = 0;

    }

    public class Siqninmodel
    {
        public string emailOrUserName{ get; set; }
        public string password{ get; set; }

    }

    public class SiqnupModel
    {

        public string email { get; set; } 

        public string userName { get; set; } 

        public string password { get; set; }

        public string repeatPassword { get; set; }

    }
}
