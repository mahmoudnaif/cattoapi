using cattoapi.ClientModles;
using cattoapi.customResponse;
using cattoapi.Interfaces;
using cattoapi.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace cattoapi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class loginController : Controller
    {
        private readonly IAuthOperationsRepo _siqiningOperationsRepo;
        private readonly IConfiguration _configuration;

        public loginController(IAuthOperationsRepo siqiningOperationsRepo,IConfiguration configuration)
        {
            _siqiningOperationsRepo = siqiningOperationsRepo;
            _configuration = configuration;
        }




        [HttpPost]
        [ProducesResponseType(201)]
        public IActionResult Login([FromBody] Siqninmodel siqninmodel)
        {
            CustomResponse<Object> customResponse = _siqiningOperationsRepo.Signin(siqninmodel);

      

            return StatusCode(customResponse.responseCode, customResponse);

        }

    }
}
