using AutoMapper;
using cattoapi.ClientModles;
using cattoapi.customResponse;
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

        public AccountsController(IAccountRepo accountRepo)
        {
            _accountRepo = accountRepo;

        }


        [HttpGet()]
        [ProducesResponseType(200, Type = typeof(IEnumerable<AccountDTO>))]
        [Authorize(Roles = "admin")]
        public IActionResult GetAccounts() 
        {
            CustomResponse<IEnumerable<AccountDTO>> customResponse = _accountRepo.GetAccounts();

           

            return StatusCode(customResponse.responseCode, customResponse);

        }


        [HttpGet("id/{strId}")]
        [ProducesResponseType(200, Type = typeof(AccountDTO))]
        [Authorize(Roles = "admin")]

        public IActionResult GetAccountById(string strId)
        {
           

            CustomResponse<AccountDTO> customResponse = _accountRepo.GetAccountById(strId);

           

            return StatusCode(customResponse.responseCode, customResponse);

        }

        [HttpGet("email/{email}")]
        [ProducesResponseType(200, Type = typeof(AccountDTO))]
        [Authorize(Roles = "admin")]

        public IActionResult GetAccountByEmail(string email)
        {
            


            CustomResponse<AccountDTO> customResponse = _accountRepo.GetAccountByEmail(email);



            return StatusCode(customResponse.responseCode, customResponse);

        }



        [HttpGet("search")]
        [ProducesResponseType(200, Type = typeof(ICollection<AccountDTO>))]
        public IActionResult SearchAccounts([FromQuery] SearchModel searchModel)
        {
           


            CustomResponse<IEnumerable<AccountDTO>> customResponse = _accountRepo.SearchAccounts(searchModel.searchQuery,searchModel.take, searchModel.skip);

          
            return StatusCode(customResponse.responseCode,customResponse);
        }

    }
}
