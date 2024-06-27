using AutoMapper;
using cattoapi.ClientModles;
using cattoapi.customResponse;
using cattoapi.DTOS;
using cattoapi.Interfaces;
using cattoapi.Models;
using cattoapi.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace cattoapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUserOperationsRepo _userOperationsRepo;
        private readonly IMapper _mapper;

        public UsersController(IUserOperationsRepo userOperationsRepo, IMapper mapper) {
            _userOperationsRepo = userOperationsRepo;
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
        public async Task<IActionResult> ChangePFP(IFormFile pfp)
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






    }
}
