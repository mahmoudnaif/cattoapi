using cattoapi.ClientModles;
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
            if (adminChangeModel == null || !Utlities.IsValidEmail(adminChangeModel.email)
                || adminChangeModel.probertyChange == "")
                return BadRequest(new {error = "body is invalid"});

            bool success = adminOperationsRepo.ChangePassword(adminChangeModel.email, adminChangeModel.probertyChange);
            if(!success)
                return StatusCode(500, new { message = "An unexpected error occurred. Please try again later." });

            return Ok();
        }


        [HttpPut("changeEmail")]
        [ProducesResponseType(200)]
        [Authorize(Roles = "admin")]
        public IActionResult ChangeEmail([FromBody] AdminChangeModel adminChangeModel)
        {
            if (adminChangeModel == null || !Utlities.IsValidEmail(adminChangeModel.email)
                || !Utlities.IsValidEmail(adminChangeModel.probertyChange))
                return BadRequest(new { error = "body is invalid" });

            bool success = adminOperationsRepo.ChangeEmail(adminChangeModel.email, adminChangeModel.probertyChange);
            if (!success)
                return StatusCode(500, new { message = "An unexpected error occurred. Please try again later." });

            return Ok();
        }



        [HttpPut("ChangeUserName")]
        [ProducesResponseType(200)]
        [Authorize(Roles = "admin")]
        public IActionResult ChangeUserName([FromBody] AdminChangeModel adminChangeModel)
        {
            if (adminChangeModel == null || !Utlities.IsValidEmail(adminChangeModel.email)
                || adminChangeModel.probertyChange == "")
                return BadRequest(new { error = "body is invalid" });

            bool success = adminOperationsRepo.ChangeUserName(adminChangeModel.email, adminChangeModel.probertyChange);
            if (!success)
                return StatusCode(500, new { message = "An unexpected error occurred. Please try again later." });

            return Ok();
        }


        [HttpPut("ChangeRole")]
        [ProducesResponseType(200)]
        [Authorize(Roles = "admin")]
        public IActionResult ChangeRole([FromBody] AdminChangeModel adminChangeModel)
        {
            if (adminChangeModel == null || !Utlities.IsValidEmail(adminChangeModel.email)
                || adminChangeModel.probertyChange == "")
                return BadRequest(new { error = "body is invalid" });

            bool success = adminOperationsRepo.ChangeRole(adminChangeModel.email, adminChangeModel.probertyChange);
            if (!success)
                return StatusCode(500, new { message = "An unexpected error occurred. Please try again later." });

            return Ok();
        }



        [HttpPut("VerifyAccount")]
        [ProducesResponseType(200)]
        [Authorize(Roles = "admin")]
        public IActionResult VerifyAccount([FromBody] string email)
        {
            if (!Utlities.IsValidEmail(email))
                return BadRequest(new { error = "body is invalid" });

            bool success = adminOperationsRepo.VerifyAccount(email);
            if (!success)
                return StatusCode(500, new { message = "An unexpected error occurred. Please try again later." });

            return Ok();
        }



        [HttpDelete("RemovePFP")]
        [ProducesResponseType(204)]
        [Authorize(Roles = "admin")]
        public IActionResult RemovePFP([FromBody] string email)
        {
            if (!Utlities.IsValidEmail(email))
                return BadRequest(new { error = "body is invalid" });

            bool success = adminOperationsRepo.RemovePFP(email);
            if (!success)
                return StatusCode(500, new { message = "An unexpected error occurred. Please try again later." });

            return NoContent();
        }


        [HttpDelete("DeleteAccount")]
        [ProducesResponseType(204)]
        [Authorize(Roles = "admin")]
        public IActionResult DeleteAccount([FromBody] string email)
        {
            if (!Utlities.IsValidEmail(email))
                return BadRequest(new { error = "body is invalid" });

            bool success = adminOperationsRepo.DeleteAccount(email);
            if (!success)
                return StatusCode(500, new { message = "An unexpected error occurred. Please try again later." });

            return NoContent();
        }

    









    }
}
