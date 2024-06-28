using AutoMapper;
using cattoapi.ClientModles;
using cattoapi.customResponse;
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


        [HttpGet("{strId}")]
        [ProducesResponseType(200, Type = typeof(AccountDTO))]
        public IActionResult GetProfiletById(string strId)
        {


            CustomResponse<ProfileDTO> customResponse = _userOperationsRepo.GetProfileById(strId);



            return StatusCode(customResponse.responseCode, customResponse);

        }

        [HttpGet("{strId}/posts")]
        public IActionResult test(string strId, [FromQuery] GetUsersPostsModel getUsersPostsModel)
        {
            int accountId;
            try
            {
                accountId = int.Parse(strId);
            }
            catch
            {   
                return StatusCode(400, new CustomResponse<AccountDTO>(400, "Id must be an integer value"));
            }
            CustomResponse<IEnumerable<PostDTO>> customResponse = _postsRepo.getUsersPosts(new GetUsersPostsIdModel { AccountId = accountId, limitGetModel = getUsersPostsModel });

            return StatusCode(customResponse.responseCode, customResponse);
        }





    }
}
