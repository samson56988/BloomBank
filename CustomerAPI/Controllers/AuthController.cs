using CustomerAPI.Dtos;
using CustomerAPI.Models.Request;
using CustomerAPI.Services;
using CustomerAPI.Services.AuthenticationServices;
using Domain.SharedModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllOrigins")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authService;
        string jsonString = "";
        ServiceResponse res = new ServiceResponse();

        public AuthController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(CreateLoginDetails request)
        {
            if (request != null)
            {
                var response = await _authService.Login(request);
                if (response.Item2 == "success")
                {
                    return Ok(response.Item1);
                }
                else
                {
                    return BadRequest(response.Item1);
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("CreateLogin")]
        public async Task<IActionResult> CreateLogin(CreateLoginDetails request)
        {
            if (request != null)
            {
                var response = await _authService.CreateLoginDetails(request);
                if (response.Item2 == "success")
                {
                    return Ok(response.Item1);
                }
                else
                {
                    return BadRequest(response.Item1);
                }
            }
            else
            {
                return BadRequest();
            }
        }


    }
}
