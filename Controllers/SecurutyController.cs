using cattoapi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace cattoapi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : Controller
    {
        private readonly IAuthOperationsRepo _authOperationsRepo;

        SecurityController(IAuthOperationsRepo authOperationsRepo) 
        {
            _authOperationsRepo = authOperationsRepo;
        }

       

    }
}
