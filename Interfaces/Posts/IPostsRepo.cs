using cattoapi.ClientModles;
using cattoapi.customResponse;
using cattoapi.DTOS;
using cattoapi.Models;

namespace cattoapi.Interfaces.Posts
{
    public interface IPostsRepo
    {

        public CustomResponse<IEnumerable<PostDTO>> getUsersPosts(GetUsersPostsIdModel getUsersPostsModel);

        public Task<CustomResponse<PostDTO>> PostaPost(int accountId, PostaPostModel postaPostModel); //IT RHYTHMS XD 

        public CustomResponse<bool> DeletePost(int accoundId, int postId);

        public Task<CustomResponse<PostDTO>> EditPost(int accountid, EditPostModel editPostModel); //This approach requiers the whole post to be sent again
                                                                                       //or the null values will be applied


    }
}
