using AutoMapper;
using cattoapi.ClientModles;
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
                return BadRequest();
            }
            Account account= _userOperationsRepo.GetData(id);


            if (account == null)
                return NotFound("wtf man how did that even happen... wrong Id in a JTW?! I'm afed");

            var accountDTO = _mapper.Map<AccountDTO>(account);
            return Ok(accountDTO);

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
