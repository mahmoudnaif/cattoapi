using cattoapi.ClientModles;
using cattoapi.CustomResponse;
using cattoapi.DTOS;
using cattoapi.Interfaces.EmailServices;
using cattoapi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Security.Claims;

namespace cattoapi.Controllers.Security
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailServicesController : Controller
    {
        private readonly IEmailServicesRepo _emailServicesRepo;

        public EmailServicesController(IEmailServicesRepo emailServicesRepo) {
            _emailServicesRepo = emailServicesRepo;
        }

        [HttpPost("SendVerficationEmail")]
        [Authorize]
        public async Task<IActionResult> SendVerficationEmail() {
            int id;
            try
            {
                id = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            }
            catch
            {
                return StatusCode(400, new CustomResponse<AccountDTO>(400, "the sent token doesn't include the account id"));

            }
            CustomResponse<bool> customResponse = await _emailServicesRepo.SendVerificationEmail(id);

            return StatusCode(customResponse.responseCode, customResponse);
        }

        [HttpPut("VerifyEmail")]
        [Authorize]
        public IActionResult VerifyEmail()
        {
            int id;
            try
            {
                id = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "accountId")?.Value);
                if (User.Claims.FirstOrDefault(c => c.Type == "Verify")?.Value != "true")
                    throw new Exception();
            }
            catch
            {
                return StatusCode(400, new CustomResponse<AccountDTO>(400, "Can't verify email using that token"));
            }
            CustomResponse<bool> customResponse = _emailServicesRepo.VerifyAccount(id);

            return StatusCode(customResponse.responseCode, customResponse);
        }


        [HttpPost("SendChangePasswordEmail")]
        public async Task<IActionResult> SendChangePasswordEmail([FromBody] string email)
        {
           
            CustomResponse<bool> customResponse = await _emailServicesRepo.SendPasswordChangeEmail(email);

            return StatusCode(customResponse.responseCode, customResponse);
        }


        [HttpPut("ChangePassword")]
        [Authorize]
        public IActionResult ChangePassword(ChangePasswordEmailModel changePasswordEmailModel)
        {
            int id;
            try
            {
                id = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "accountId")?.Value);
                if (User.Claims.FirstOrDefault(c => c.Type == "changepassword")?.Value != "true")
                    throw new Exception();
            }
            catch
            {
                return StatusCode(400, new CustomResponse<AccountDTO>(400, "Can't verify email using that token"));
            }
            CustomResponse<bool> customResponse = _emailServicesRepo.ChangePassword(id, changePasswordEmailModel.newPassword,changePasswordEmailModel.repeatNewPassword);

            return StatusCode(customResponse.responseCode, customResponse);
        }



    }
}
