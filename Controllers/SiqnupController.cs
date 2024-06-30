using cattoapi.ClientModles;
using cattoapi.CustomResponse;
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
        [ProducesResponseType(201)]
        public async Task<IActionResult> Siqnup([FromBody] SiqnupModel signupModel)
        {
          CustomResponse<bool> customResponse= await _siqiningOperationsRepo.CreateAccountAsync(signupModel);
          
           

            return StatusCode(customResponse.responseCode, customResponse);

        }




    }
}
