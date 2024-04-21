using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiddleWareAPI.Models.Dto;
using MiddleWareAPI.Services.AccountService;
using MiddleWareDomain.Models;

namespace MiddleWareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }


        [Produces("application/json", "application/xml")]
        [Consumes("application/json", "application/xml")]
        [HttpPost("CreateAccount")]
        public async Task<IActionResult> CreateAccount(CreateCustomerDto customer)
        {
            var createAccount = await _accountService.CreateAccountService(customer);
            if(createAccount.Item2 == "success")
            {
                return Ok(createAccount.Item1);
            }
            else
            {
                return BadRequest(createAccount.Item1);
            }
        }

        [Produces("application/json", "application/xml")]
        [Consumes("application/json", "application/xml")]
        [HttpPost("SetAccountOnPND")]
        public async Task<IActionResult> SetAccountToPND(string AccountNO, bool hasPND)
        {
            var createAccount = await _accountService.SetAccountOnPND(AccountNO,hasPND);
            if (createAccount.Item2 == "success")
            {
                return Ok(createAccount.Item1);
            }
            else
            {
                return BadRequest(createAccount.Item1);
            }
        }

        [Produces("application/json", "application/xml")]
        [Consumes("application/json", "application/xml")]
        [HttpGet("GetAccountDetails")]
        public async Task<IActionResult> GetAccountDetails(string accountNO)
        {
            var createAccount = await _accountService.GetAccountDetails(accountNO);
            if (createAccount.Item2 == "success")
            {
                return Ok(createAccount.Item1);
            }
            else
            {
                return BadRequest(createAccount.Item1);
            }
        }

        [Produces("application/json", "application/xml")]
        [Consumes("application/json", "application/xml")]
        [HttpGet("FundAccount")]
        public async Task<IActionResult> FundAccount(string accountNO, decimal amount)
        {
            var createAccount = await _accountService.FundAccount(accountNO,amount);
            if (createAccount.Item2 == "success")
            {
                return Ok(createAccount.Item1);
            }
            else
            {
                return BadRequest(createAccount.Item1);
            }
        }

        [Produces("application/json", "application/xml")]
        [Consumes("application/json", "application/xml")]
        [HttpPost("CreateCorporateWalletAccount")]
        public async Task<IActionResult> CreateCorporateWalletAccount(CreateWalletAccountDto corprate)
        {
            var createAccount = await _accountService.CreateCorporateWalletAccount(corprate);
            if (createAccount.Item2 == "success")
            {
                return Ok(createAccount.Item1);
            }
            else
            {
                return BadRequest(createAccount.Item1);
            }
        }

        [Produces("application/json", "application/xml")]
        [Consumes("application/json", "application/xml")]
        [HttpGet("GetCorporateWalletAccount")]
        public async Task<IActionResult> GetCorporateWalletAccount(string businessKey)
        {
            var createAccount = await _accountService.GetCorporateWalletAccount(businessKey);
            if (createAccount.Item2 == "success")
            {
                return Ok(createAccount.Item1);
            }
            else
            {
                return BadRequest(createAccount.Item1);
            }
        }

        [Produces("application/json", "application/xml")]
        [Consumes("application/json", "application/xml")]
        [HttpPost("DeactivateCorporateWalletAccount")]
        public async Task<IActionResult> DeactivateCorporateWalletAccount(string businessKey)
        {
            var createAccount = await _accountService.DeactivateCorporateWalletAccount(businessKey);
            if (createAccount.Item2 == "success")
            {
                return Ok(createAccount.Item1);
            }
            else
            {
                return BadRequest(createAccount.Item1);
            }
        }

        [Produces("application/json", "application/xml")]
        [Consumes("application/json", "application/xml")]
        [HttpPost("ActivateCorporateWalletAccount")]
        public async Task<IActionResult> ActivateCorporateWalletAccount(string businessKey)
        {
            var createAccount = await _accountService.ActivateCorporateWalletAccount(businessKey);
            if (createAccount.Item2 == "success")
            {
                return Ok(createAccount.Item1);
            }
            else
            {
                return BadRequest(createAccount.Item1);
            }
        }

        [Produces("application/json", "application/xml")]
        [Consumes("application/json", "application/xml")]
        [HttpPost("CreateWalletAccount")]
        public async Task<IActionResult> CreateWalletAccount(WalletAccountDto virtualAccount)
        {
            var createAccount = await _accountService.CreateWalletAccount(virtualAccount);
            if (createAccount.Item2 == "success")
            {
                return Ok(createAccount.Item1);
            }
            else
            {
                return BadRequest(createAccount.Item1);
            }
        }

        [Produces("application/json", "application/xml")]
        [Consumes("application/json", "application/xml")]
        [HttpPost("CreateVirtualCard")]
        public async Task<IActionResult> CreateVirtualCard(VirtualCardDetailsDto virtualAccount)
        {
            var createAccount = await _accountService.CreateVirtualCard(virtualAccount);
            if (createAccount.Item2 == "success")
            {
                return Ok(createAccount.Item1);
            }
            else
            {
                return BadRequest(createAccount.Item1);
            }
        }

        [Produces("application/json", "application/xml")]
        [Consumes("application/json", "application/xml")]
        [HttpPost("BlockVirtualCard")]
        public async Task<IActionResult> BlockVirtualCard(string customerId, string cardId)
        {
            var createAccount = await _accountService.BlockVirtualCard(customerId, cardId);
            if (createAccount.Item2 == "success")
            {
                return Ok(createAccount.Item1);
            }
            else
            {
                return BadRequest(createAccount.Item1);
            }
        }

        [Produces("application/json", "application/xml")]
        [Consumes("application/json", "application/xml")]
        [HttpGet("GetCustomerActiveCard")]
        public async Task<IActionResult> GetCustomerActiveCard(string customerId)
        {
            var createAccount = await _accountService.GetCustomerActiveCards(customerId);
            if (createAccount.Item2 == "success")
            {
                return Ok(createAccount.Item1);
            }
            else
            {
                return BadRequest(createAccount.Item1);
            }
        }

        [Produces("application/json", "application/xml")]
        [Consumes("application/json", "application/xml")]
        [HttpPost("ActivateVirtualCard")]
        public async Task<IActionResult> ActivateVirtualCard(string customerId , string cardPin, string cardId)
        {
            var createAccount = await _accountService.ActivateVirtualCard(customerId,cardPin, cardId);
            if (createAccount.Item2 == "success")
            {
                return Ok(createAccount.Item1);
            }
            else
            {
                return BadRequest(createAccount.Item1);
            }
        }


    }
}
