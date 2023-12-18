using Microsoft.AspNetCore.Mvc;
using ProjectHorizon.Microservice.Account.Components.Base;
using ProjectHorizon.Microservice.Account.Components.Models;

namespace ProjectHorizon.Microservice.Account.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(IDependencies dependencies) : base(dependencies)
        {
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] LoginModel query)
        {
            return await GetQueryResultResponse(query);
        }

        [HttpPost("Register")]
        public async Task<ActionResult> Register([FromBody] RegisterModel query)
        {
            return await GetQueryResultResponse(query);
        }

        [HttpPost("Recovery")]
        public async Task<ActionResult> Recovery([FromBody] RecoveryModel query)
        {
            return await GetQueryResultResponse(query);
        }
    }
}
