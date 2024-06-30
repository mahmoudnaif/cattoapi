using cattoapi.ClientModles;
using cattoapi.CustomResponse;
using cattoapi.DTOS;
using cattoapi.Models;

namespace cattoapi.Interfaces.Likes
{
    public interface ILikesRepo
    {
        public CustomResponse<IEnumerable<ProfileDTO>> GetPostLikes(GetFromPostModel getFromPostModel);

        public CustomResponse<IEnumerable<PostDTO>> GetUserLikes(int accountId, TakeSkipModel takeSkipModel);

        public CustomResponse<bool> AddLike(int accountId, int postId);

        public CustomResponse<bool> RemoveLike(int accountId, int postId);
    }
}
