using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiddleWareAPI.Models.Dto;
using MiddleWareAPI.Services.AccountService;
using MiddleWareAPI.Services.TransferService;

namespace MiddleWareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransferController : ControllerBase
    {
        private readonly ITransferService _service;

        public  TransferController(ITransferService service)
        {
            _service = service;
        }


        [Produces("application/json", "application/xml")]
        [Consumes("application/json", "application/xml")]
        [HttpPost("BankTransfer")]
        public async Task<IActionResult> BankTransfer(InitializeBankTransfer transfer)
        {
            var createAccount = await _service.InitiateBankTransfer(transfer);
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
