using cattoapi.ClientModles;
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
        [ProducesResponseType(201, Type = typeof(string))]
        public IActionResult Login([FromBody] Siqninmodel siqninmodel)
        {
            Account account= _siqiningOperationsRepo.Signin(siqninmodel);

            if (account == null)
                return Unauthorized(new { error = "invalid data"});

          string JWTToken = utlities.Utlities.generateLoginJWT((int)account.AccountId, account.Role, _configuration["Jwt:Key"]);


            return CreatedAtAction(null,new {token = JWTToken});

        }

    }
}
