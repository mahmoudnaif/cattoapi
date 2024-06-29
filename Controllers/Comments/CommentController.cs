using cattoapi.ClientModles;
using cattoapi.customResponse;
using cattoapi.DTOS;
using cattoapi.Interfaces.Comments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace cattoapi.Controllers.Comments
{
    [Route("api/")]
    [ApiController]
    public class CommentController : Controller
    {
        private readonly ICommentsRepo _commentsRepo;

       public CommentController(ICommentsRepo commentsRepo) {
            _commentsRepo = commentsRepo;
                 }


        [HttpGet("User/Comments")]
        [Authorize]
        public IActionResult ViewUserComments([FromQuery] TakeSkipModel takeSkipModel)
        {
            int accountId;
            try
            {
                accountId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            }
            catch
            {
                CustomResponse<IEnumerable<CommentDTO>> customResponsec = new CustomResponse<IEnumerable<CommentDTO>>(400,"Invalid id");
                return StatusCode(customResponsec.responseCode, customResponsec);
            }

            CustomResponse<IEnumerable<CommentDTO>> customResponse = _commentsRepo.GetUserComments(accountId, takeSkipModel);

            return StatusCode(customResponse.responseCode, customResponse);


        }



        [HttpGet("Posts/Comments")]
        
        public IActionResult ViewPostComments([FromQuery] GetCommentsModel getCommentsModel)
        {
          
            CustomResponse<IEnumerable<CommentDTO>> customResponse = _commentsRepo.GetPostComments(getCommentsModel);

            return StatusCode(customResponse.responseCode, customResponse);


        }



        [HttpPost("User/Comments")]
        [Authorize]
        public IActionResult PostComment([FromBody] PostCommentModel postCommentModel)
        {
            int accountId;
            try
            {
                accountId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            }
            catch
            {
                CustomResponse<CommentDTO> customResponsec = new CustomResponse<CommentDTO>(400, "Invalid id");
                return StatusCode(customResponsec.responseCode, customResponsec);
            }

            CustomResponse<CommentDTO> customResponse = _commentsRepo.PostComment(accountId, postCommentModel);

            return StatusCode(customResponse.responseCode, customResponse);


        }


        [HttpPut("User/Comments")]
        [Authorize]
        public IActionResult EditComment([FromBody] EditCommentModel editCommentModel) {
            int accountId;
            try
            {
                accountId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            }
            catch
            {
                CustomResponse<CommentDTO> customResponsec = new CustomResponse<CommentDTO>(400, "Invalid id");
                return StatusCode(customResponsec.responseCode, customResponsec);
            }

            CustomResponse<CommentDTO> customResponse = _commentsRepo.EditComment(accountId, editCommentModel);

            return StatusCode(customResponse.responseCode, customResponse);


        }



        [HttpDelete("User/Comments")]
        [Authorize]
        public IActionResult DeleteComment([FromBody] int commentId)
        {
            int accountId;
            try
            {
                accountId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            }
            catch
            {
                CustomResponse<bool> customResponsec = new CustomResponse<bool>(400, "Invalid id");
                return StatusCode(customResponsec.responseCode, customResponsec);
            }

            CustomResponse<bool> customResponse = _commentsRepo.DeleteComment(accountId, commentId, User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value);

            return StatusCode(customResponse.responseCode, customResponse);


        }






    }
}
