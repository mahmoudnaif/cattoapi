using cattoapi.ClientModles;
using cattoapi.customResponse;
using cattoapi.DTOS;

namespace cattoapi.Interfaces.Comments
{
    public interface ICommentsRepo
    {  
        public CustomResponse<IEnumerable<CommentDTO>> GetPostComments(GetCommentsModel getComments);

        public CustomResponse<IEnumerable<CommentDTO>> GetUserComments(GetCommentsModel getComments);

        public CustomResponse<CommentDTO> PostComment(int accountId, PostCommentModel postCommentModel);

        public CustomResponse<CommentDTO> EditComment(int accountId, EditCommentModel editCommentModel);

        public CustomResponse<bool> DeleteComment(int accountId, int commentId, string role);


    }
}