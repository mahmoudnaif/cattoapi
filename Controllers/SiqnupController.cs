using cattoapi.ClientModles;
using cattoapi.Interfaces;
using cattoapi.Models;
using Microsoft.AspNetCore.Mvc;

namespace cattoapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SiqnupController : Controller
    {
        private readonly IAccountRepo accountRepo;

        public SiqnupController(IAccountRepo accountRepo)
        {
            this.accountRepo = accountRepo;
        }


     




        [HttpPost]
        [ProducesResponseType(200, Type = typeof(SiqnupModel))]
        public async Task<IActionResult> Siqnup([FromBody] SiqnupModel signupModel)
        {
           bool success = await accountRepo.CreateAccountAsync(signupModel);
            if (!success)
                return BadRequest(new { error = "something went wrong" });
            
            

            return Ok(success);

        }




    }
}
