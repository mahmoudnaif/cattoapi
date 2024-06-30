using cattoapi.ClientModles;
using cattoapi.CustomResponse;
using cattoapi.DTOS;
using cattoapi.Interfaces.Likes;
using cattoapi.Repos.Likes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace cattoapi.Controllers.Likes
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikesController : Controller
    {
        private readonly ILikesRepo _likesRepo;

        public LikesController(ILikesRepo likesRepo)
        {
            _likesRepo = likesRepo;
        }



        [HttpGet("post")]
        public IActionResult GetPostLikes([FromQuery]GetFromPostModel getFromPostModel)
        {
        var customResponse = _likesRepo.GetPostLikes(getFromPostModel);

            return StatusCode(customResponse.responseCode,customResponse);
        }

        [HttpGet("user")]
        [Authorize]
        public IActionResult GetUsertLikes([FromQuery]TakeSkipModel takeSkipModel)
        {
            int id;
            try
            {
                id = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            }
            catch
            {
                return StatusCode(400, new CustomResponse<AccountDTO>(400, "the sent token doesn't include the account id"));

            }
            var customResponse = _likesRepo.GetUserLikes(id, takeSkipModel);

            return StatusCode(customResponse.responseCode, customResponse);
        }

        [HttpPost]
        [Authorize]
        public IActionResult LikePost([FromBody]int postId)
        {
            int id;
            try
            {
                id = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            }
            catch
            {
                return StatusCode(400, new CustomResponse<AccountDTO>(400, "the sent token doesn't include the account id"));

            }
            var customResponse = _likesRepo.AddLike(id, postId);

            return StatusCode(customResponse.responseCode, customResponse);
        }

        [HttpDelete]
        [Authorize]
        public IActionResult RemoveLike([FromBody] int postId)
        {
            int id;
            try
            {
                id = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            }
            catch
            {
                return StatusCode(400, new CustomResponse<AccountDTO>(400, "the sent token doesn't include the account id"));

            }
            var customResponse = _likesRepo.RemoveLike(id, postId);

            return StatusCode(customResponse.responseCode, customResponse);
        }
    }


}