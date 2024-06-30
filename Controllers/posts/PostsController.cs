using cattoapi.ClientModles;
using cattoapi.CustomResponse;
using cattoapi.DTOS;
using cattoapi.Interfaces.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace cattoapi.Controllers.posts
{
    [Route("api/User/[controller]")]
    [ApiController]
    public class PostsController : Controller
    {
        private readonly IPostsRepo _postsRepo;

        public PostsController(IPostsRepo postsRepo) {
            _postsRepo = postsRepo;
        }



        [HttpGet("GetPosts")]
        [Authorize]
        public IActionResult GetPosts([FromQuery] TakeSkipModel getUsersPostsModel)
        {
        
              string  accountId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            
         
            CustomResponse<IEnumerable<PostDTO>> customResponse = _postsRepo.getUsersPosts(new GetUsersPostsIdModel { AccountIdOrUserame = accountId, limitGetModel = getUsersPostsModel });

            return StatusCode(customResponse.responseCode, customResponse);
        }
        
     


        [HttpPost("PostaPost")]
        [Authorize]
        public async Task<IActionResult> PostaPost([FromBody] PostaPostModel postaPostModel)
        {
            int accountId;
            try
            {
                accountId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            }
            catch
            {
                return StatusCode(400, new CustomResponse<AccountDTO>(400, "the sent token doesn't include the account id"));
            }
            CustomResponse<PostDTO> customResponse = await _postsRepo.PostaPost(accountId,postaPostModel);

            return StatusCode(customResponse.responseCode, customResponse);
        }

        [HttpPost("EditaPost")]
        [Authorize]
        public async Task<IActionResult> EditaPost([FromBody] EditPostModel editPostModel)
        {
            int accountId;
            try
            {
                accountId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            }
            catch
            {
                return StatusCode(400, new CustomResponse<AccountDTO>(400, "the sent token doesn't include the account id"));
            }
            CustomResponse<PostDTO> customResponse = await _postsRepo.EditPost(accountId,editPostModel);

            return StatusCode(customResponse.responseCode, customResponse);
        }


        [HttpDelete("DeletePost")]
        [Authorize]
        public async Task<IActionResult> DeletePost([FromBody] int postId)
        {
            int accountId;
            try
            {
                accountId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            }
            catch
            {
                return StatusCode(400, new CustomResponse<AccountDTO>(400, "the sent token doesn't include the account id"));
            }
            CustomResponse<bool> customResponse =  _postsRepo.DeletePost(accountId, postId);

            return StatusCode(customResponse.responseCode, customResponse);
        }


    }
}
