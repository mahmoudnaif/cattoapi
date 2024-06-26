using cattoapi.ClientModles;
using cattoapi.Interfaces;
using cattoapi.Models;
using cattoapi.Repos;
using cattoapi.utlities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cattoapi.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : Controller
    {

        private readonly IAccountRepo accountRepo;

        public AccountsController(IAccountRepo accountRepo)
        {
            this.accountRepo = accountRepo;
        }


        [HttpGet()]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Account>))]
        [Authorize(Roles = "admin")]
        public IActionResult GetAccounts() 
        {
            var accounts = accountRepo.GetAccounts();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(accounts);

        }


        [HttpGet("id/{strId}")]
        [ProducesResponseType(200, Type = typeof(Account))]
        [Authorize(Roles = "admin")]

        public IActionResult GetAccountById(string strId)
        {
            if(!int.TryParse(strId, out int id))
                return BadRequest(new { error = "Invalid id format. Please enter a valid integer id." });
            
            var account = accountRepo.GetAccountById(id);

           

            if (account == null)
                return NotFound();

            return Ok(account);

        }

        [HttpGet("email/{email}")]
        [ProducesResponseType(200, Type = typeof(Account))]
        [Authorize(Roles = "admin")]

        public IActionResult GetAccountByEmail(string email)
        {
            if(!utlities.utlities.IsValidEmail(email))
                return BadRequest(new { error = "Invalid email format. Please enter a valid email." });


            var account = accountRepo.GetAccountByEmail(email);



            if (account == null)
                return NotFound();

            return Ok(account);

        }



        [HttpGet("search")]
        [ProducesResponseType(200, Type = typeof(ICollection<Account>))]
        public IActionResult SearchAccounts([FromQuery] SearchModel searchModel)
        {
            if (searchModel.take == 0)
                return NotFound();


            if(searchModel.take<0 || searchModel.skip < 0)
            {
                return BadRequest(new { error = "Invalid numbers take and skip must be 0 or more" });

            }

            var results = accountRepo.SearchAccounts(searchModel.searchQuery,searchModel.take, searchModel.skip);

            if(results == null)
                return NotFound();


            return Ok(results);
        }

    }
}
