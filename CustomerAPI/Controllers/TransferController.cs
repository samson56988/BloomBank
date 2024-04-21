using CustomerAPI.Dtos;
using CustomerAPI.Models.Request;
using CustomerAPI.Models.Response;
using CustomerAPI.Services.TransferServices;
using Domain.SharedModels;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace CustomerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransferController : ControllerBase
    {
        private readonly ITransferService _transferService;
        string jsonString = "";
        ServiceResponse res = new ServiceResponse();



        public TransferController(ITransferService transfService)
        {
            _transferService = transfService;        
        }

        [HttpPost("Transfer")]
        public async Task<IActionResult> Transfer(TransferRequest request)
        {
            if (request != null)
            {
                var response = await _transferService.TransferRequest(request);
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
