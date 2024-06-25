using cattoapi.Interfaces;
using cattoapi.Models;
using cattoapi.Repos;
using Microsoft.AspNetCore.Mvc;

namespace cattoapi.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {

        private readonly IAccountRepo accountRepo;
        private readonly CattoDbContext context;

        public AccountController(IAccountRepo accountRepo, CattoDbContext context)
        {
            this.accountRepo = accountRepo;
            this.context = context;
        }


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Account>))]
        public IActionResult GetAccounts() 
        {
            var accounts = context.Accounts.Where(u => u.AccountId == 1).Select(u => u.LikedPosts).ToList();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(accounts);

        }
    }
}
