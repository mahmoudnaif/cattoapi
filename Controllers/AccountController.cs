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

        private readonly CattoDbContext _context;

        public AccountController(CattoDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Account>))]
        public IActionResult GetAccounts() 
        {
            var accounts = _context.Accounts.Where(p => p.AccountId == 1).Select(p => p.Posts);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(accounts);

        }
    }
}
