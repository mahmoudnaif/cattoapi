using AutoMapper;
using cattoapi.ClientModles;
using cattoapi.DTOS;
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

        private readonly IAccountRepo _accountRepo;
        private readonly IMapper _mapper;

        public AccountsController(IAccountRepo accountRepo, IMapper mapper)
        {
            _accountRepo = accountRepo;
            _mapper = mapper;
        }


        [HttpGet()]
        [ProducesResponseType(200, Type = typeof(IEnumerable<AccountDTO>))]
        [Authorize(Roles = "admin")]
        public IActionResult GetAccounts() 
        {
            var accounts = _accountRepo.GetAccounts();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            IEnumerable<AccountDTO> accountDTO = _mapper.Map<IEnumerable<AccountDTO>>(accounts);
            return Ok(accountDTO);

        }


        [HttpGet("id/{strId}")]
        [ProducesResponseType(200, Type = typeof(AccountDTO))]
        [Authorize(Roles = "admin")]

        public IActionResult GetAccountById(string strId)
        {
            if(!int.TryParse(strId, out int id))
                return BadRequest(new { error = "Invalid id format. Please enter a valid integer id." });
            
            var account = _accountRepo.GetAccountById(id);

           

            if (account == null)
                return NotFound();

            var accountDTO = _mapper.Map<AccountDTO>(account);

            return Ok(accountDTO);

        }

        [HttpGet("email/{email}")]
        [ProducesResponseType(200, Type = typeof(AccountDTO))]
        [Authorize(Roles = "admin")]

        public IActionResult GetAccountByEmail(string email)
        {
            if(!utlities.Utlities.IsValidEmail(email))
                return BadRequest(new { error = "Invalid email format. Please enter a valid email." });


            var account = _accountRepo.GetAccountByEmail(email);



            if (account == null)
                return NotFound();


            var accountDTO = _mapper.Map<AccountDTO>(account);

            return Ok(accountDTO);

        }



        [HttpGet("search")]
        [ProducesResponseType(200, Type = typeof(ICollection<AccountDTO>))]
        public IActionResult SearchAccounts([FromQuery] SearchModel searchModel)
        {
            if (searchModel.take == 0)
                return NotFound();


            if(searchModel.take<0 || searchModel.skip < 0)
                return BadRequest(new { error = "Invalid numbers take and skip must be 0 or more" });
            

            var results = _accountRepo.SearchAccounts(searchModel.searchQuery,searchModel.take, searchModel.skip);

            if(results == null)
                return NotFound();

            IEnumerable<AccountDTO> accountDTO = _mapper.Map<IEnumerable<AccountDTO>>(results);
            return Ok(accountDTO);
        }

    }
}
