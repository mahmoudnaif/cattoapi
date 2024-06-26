using cattoapi.ClientModles;
using cattoapi.Interfaces;
using cattoapi.Models;
using cattoapi.Repos;
using Microsoft.AspNetCore.Mvc;

namespace cattoapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SiqnupController : Controller
    {
        private readonly IAuthOperationsRepo _siqiningOperationsRepo;

        public SiqnupController(IAuthOperationsRepo siqiningOperationsRepo)
        {
            _siqiningOperationsRepo = siqiningOperationsRepo;
        }


     




        [HttpPost]
        [ProducesResponseType(201, Type = typeof(bool))]
        public async Task<IActionResult> Siqnup([FromBody] SiqnupModel signupModel)
        {
           bool success = await _siqiningOperationsRepo.CreateAccountAsync(signupModel);
            if (!success)
                return BadRequest(new { error = "something went wrong" });
            
            

            return CreatedAtAction(null,new { success = "true" });

        }




    }
}
