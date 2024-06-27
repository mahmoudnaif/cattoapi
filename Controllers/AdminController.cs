using cattoapi.ClientModles;
using cattoapi.customResponse;
using cattoapi.Interfaces;
using cattoapi.utlities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cattoapi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly IAdminOperationsRepo adminOperationsRepo;

        public AdminController(IAdminOperationsRepo adminOperationsRepo) 
        {
            this.adminOperationsRepo = adminOperationsRepo;
        }

        [HttpPut("Changepassword")]
        [ProducesResponseType(200)]
        [Authorize(Roles = "admin")]
        public IActionResult ChangePassword([FromBody] AdminChangeModel adminChangeModel)
        {
           

            CustomResponse<bool> customResponse = adminOperationsRepo.ChangePassword(adminChangeModel);


            return StatusCode(customResponse.responseCode, customResponse);
        }


        [HttpPut("changeEmail")]
        [ProducesResponseType(200)]
        [Authorize(Roles = "admin")]
        public IActionResult ChangeEmail([FromBody] AdminChangeModel adminChangeModel)
        {
          


            CustomResponse<bool> customResponse = adminOperationsRepo.ChangeEmail(adminChangeModel);


            return StatusCode(customResponse.responseCode, customResponse);
        }



        [HttpPut("ChangeUserName")]
        [ProducesResponseType(200)]
        [Authorize(Roles = "admin")]
        public IActionResult ChangeUserName([FromBody] AdminChangeModel adminChangeModel)
        {



            CustomResponse<bool> customResponse = adminOperationsRepo.ChangeUserName(adminChangeModel);


            return StatusCode(customResponse.responseCode, customResponse);
        }


        [HttpPut("ChangeRole")]
        [ProducesResponseType(200)]
        [Authorize(Roles = "admin")]
        public IActionResult ChangeRole([FromBody] AdminChangeModel adminChangeModel)
        {



            CustomResponse<bool> customResponse = adminOperationsRepo.ChangeRole(adminChangeModel);

            return StatusCode(customResponse.responseCode, customResponse);
        }



        [HttpPut("VerifyAccount")]
        [ProducesResponseType(200)]
        [Authorize(Roles = "admin")]
        public IActionResult VerifyAccount([FromBody] string email)
        {



            CustomResponse<bool> customResponse = adminOperationsRepo.VerifyAccount(email);


            return StatusCode(customResponse.responseCode, customResponse);
        }



        [HttpDelete("RemovePFP")]
        [ProducesResponseType(204)]
        [Authorize(Roles = "admin")]
        public IActionResult RemovePFP([FromBody] string email)
        {



            CustomResponse<bool> customResponse = adminOperationsRepo.RemovePFP(email);


            return StatusCode(customResponse.responseCode, customResponse);
        }


        [HttpDelete("DeleteAccount")]
        [ProducesResponseType(204)]
        [Authorize(Roles = "admin")]
        public IActionResult DeleteAccount([FromBody] string email)
        {



            CustomResponse<bool> customResponse = adminOperationsRepo.DeleteAccount(email);

            return StatusCode(customResponse.responseCode, customResponse);
        }

    









    }
}
