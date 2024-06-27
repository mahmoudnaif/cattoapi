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

    public class AdminChangeModel
    {

        public string email { get; set; }

        public string probertyChange { get; set; } = "";

    }


    public class ChangePasswordModel
    {
        public string oldPassword { get; set; }

        public string newPassword { get; set; } = "";
    }

    public class PostaPostModel
    {
        public string? PostText { get; set; }
 
       public string? PostPictrue { get; set; }

    }

    public class EditPostModel
    {
        public int postId { get; set;}
        
        public PostaPostModel data { get; set; }  
    }

   



    public class GetUsersPostsIdModel
    {
        public int AccountId { get; set; }

        public GetUsersPostsModel limitGetModel { get; set; } = new GetUsersPostsModel();

    }

    public class GetUsersPostsModel
    {
        public int take { get; set; } = 10;
        public int skip { get; set; } = 0;

    }




}
