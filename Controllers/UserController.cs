using AutoMapper;
using cattoapi.ClientModles;
using cattoapi.CustomResponse;
using cattoapi.DTOS;
using cattoapi.Interfaces;
using cattoapi.Interfaces.Posts;
using cattoapi.Models;
using cattoapi.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace cattoapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserOperationsRepo _userOperationsRepo;
        private readonly IPostsRepo _postsRepo;
        private readonly IMapper _mapper;

        public UserController(IUserOperationsRepo userOperationsRepo,IPostsRepo postsRepo, IMapper mapper) {
            _userOperationsRepo = userOperationsRepo;
            _postsRepo = postsRepo;
            _mapper = mapper;
        }


        [HttpGet("GetData")]
        [Authorize]
        public IActionResult GetUserData() {
            int id;
            try
            {
                id = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            }
            catch
            {
                return StatusCode(400, new CustomResponse<AccountDTO>(400,"the sent token doesn't include the account id"));

            }
            CustomResponse<AccountDTO> customResponse= _userOperationsRepo.GetData(id);


            return StatusCode(customResponse.responseCode, customResponse);

        }

        [HttpPut("ChangePassword")]
        [Authorize]
        public IActionResult ChangePassword([FromBody] ChangePasswordModel changePasswordModel) {
            int id;
            try
            {
                id = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            }
            catch
            {
                return StatusCode(400, new CustomResponse<AccountDTO>(400, "the sent token doesn't include the account id"));
            }

            CustomResponse<bool> customResponse = _userOperationsRepo.ChangePassword(id, changePasswordModel.oldPassword, changePasswordModel.newPassword);

            

            return StatusCode(customResponse.responseCode,customResponse);
        }


        [HttpPut("ChangePFP")]
        [Authorize]
        public async Task<IActionResult> ChangePFP([FromBody]string pfp)
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

            CustomResponse<bool> customResponse = await _userOperationsRepo.Changepfp(id, pfp);

           

            return StatusCode(customResponse.responseCode, customResponse);
        }


        [HttpGet("{strIdOrUsername}")]
        [ProducesResponseType(200, Type = typeof(AccountDTO))]
        public IActionResult GetProfiletById(string strIdOrUsername)
        {


            CustomResponse<ProfileDTO> customResponse = _userOperationsRepo.GetProfileById(strIdOrUsername);



            return StatusCode(customResponse.responseCode, customResponse);

        }

        [HttpGet("{strIdOrUsername}/posts")]
        public IActionResult test(string strIdOrUsername, [FromQuery] TakeSkipModel getUsersPostsModel)
        {
          
            CustomResponse<IEnumerable<PostDTO>> customResponse = _postsRepo.getUsersPosts(new GetUsersPostsIdModel { AccountIdOrUserame = strIdOrUsername, limitGetModel = getUsersPostsModel });

            return StatusCode(customResponse.responseCode, customResponse);
        }





    }
}
