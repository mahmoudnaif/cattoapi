using cattoapi.ClientModles;
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

        public UsersController(IUserOperationsRepo userOperationsRepo) {
            _userOperationsRepo = userOperationsRepo;
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
                return BadRequest();
            }
            Account account= _userOperationsRepo.GetData(id);


            if (account == null)
                return NotFound("wtf man how did that even happen... wrong Id in a JTW?! I'm afed");

            //map account to an accountDto not all data can be sent
            return Ok(account);

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
                return BadRequest();
            }
            bool success = _userOperationsRepo.ChangePassword(id, changePasswordModel.oldPassword, changePasswordModel.newPassword);

            if (!success)
                return BadRequest();

            return Ok();
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
                return BadRequest();
            }

            bool success = await _userOperationsRepo.Changepfp(id, pfp);

            if (!success)
                return BadRequest();

            return Ok();
        }






    }
}
