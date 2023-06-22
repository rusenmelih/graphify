using Business.Abstract;
using Entity.FTM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Graphfy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest register)
        {
            var result = await this.authenticationService.RegisterUser(register);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            var result = await this.authenticationService.LoginUser(login);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }


    }
}
