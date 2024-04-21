using CustomerAPI.Models.Request;
using CustomerAPI.Services;
using Domain.SharedModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllOrigins")]
    public class AccountController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        string jsonString = "";
        ServiceResponse res = new ServiceResponse();

        public AccountController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("GetAccountDetail")]
        public async Task<IActionResult> GetAccountDetails(string accountId)
        {
            if (accountId != null)
            {
                var response = await _customerService.GetCustomerAccountDetails(accountId);
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

        [HttpPost("CreateAccount")]
        public async Task<IActionResult> CreateAccount(CreateCustomerRequest customer)
        {
            if (customer != null)
            {
                var response = await _customerService.CreateAccount(customer);
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

        [HttpPost("ActivateAccount")]
        public async Task<IActionResult> ActivateAccount(string accountNo)
        {
            if (accountNo != null)
            {
                var response = await _customerService.ActivateCustomer(accountNo);
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
