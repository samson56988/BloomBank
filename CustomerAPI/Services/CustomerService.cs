
using ApiCaller;
using AutoMapper;
using CustomerAPI.ApiServices;
using CustomerAPI.Dtos;
using CustomerAPI.Models.Request;
using CustomerAPI.Models.Response;
using DataBase.Models;
using Domain.CustomerDomains.Services.Interfaces;
using Domain.SharedModels;
using Microsoft.Extensions.Logging;

namespace CustomerAPI.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;
        ServiceResponse res = new ServiceResponse();
        string message = "";
        string error = "";

        public CustomerService(ICustomerRepository customerRepository,IMapper mapper,IAccountService accountService)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _accountService = accountService;
        }

        public async Task<Tuple<object, string>> ActivateCustomer(string accountNo)
        {
            try
            {
                var createaccount = await _customerRepository.ActivateCustomer(accountNo);
                if (createaccount)
                {
                    message = "Account Activated Successfully";
                    res.success = true;
                    res.data = null;
                    res.message = message;
                    return new Tuple<object, string>(res, "success");
                }
                else
                {
                    message = "Error occurred while processing";
                    res.success = false;
                    res.data = null;
                    res.message = message;
                    return new Tuple<object, string>(res, "error");
                }
            }
            catch
            {
                message = "Error occurred while processing request";
                res.success = false;
                res.data = null;
                res.message = message;
                return new Tuple<object, string>(res, "error");
            }
        }

        public async Task<Tuple<object, string>> CreateAccount(CreateCustomerRequest request)
        {
            try
            {
                var createaccount =  await _accountService.CreateCustomer(request);

                if(createaccount.Success)
                {
                    message = "Account Created Successfully";
                    res.success = true;
                    res.data = createaccount.Data;
                    res.message = message;
                    return new Tuple<object, string>(res, "success");
                }
                else
                {
                    message = createaccount.Message;
                    res.success = false;
                    res.data = null;
                    res.message = message;
                    return new Tuple<object, string>(res, "error");
                }
            }
            catch
            {
                message = "Error occurred while processing request";
                res.success = false;
                res.data = null;
                res.message = message;
                return new Tuple<object, string>(res, "error");
            }
        }

        public async Task<Tuple<object, string>> GetCustomerAccountDetails(string accountNo)
        {
            try
            {
                var accountDetails = await _accountService.GetAccountDetailsAsync(accountNo);

                if (accountDetails == null)
                {
                    message = "Account Does Not Exist";
                    res.success = false;
                    res.data = null;
                    res.message = message;
                    return new Tuple<object, string>(res, "error");
                }
                else
                {
                    message = "Account Details Fetched Successfully";
                    res.success = true;
                    res.data = accountDetails.Data;
                    res.message = message;
                    return new Tuple<object, string>(res, "success");
                }
            }
            catch
            {
                message = "Error occured while processing request";
                res.success = false;
                res.data = null;
                res.message = message;
                return new Tuple<object, string>(res, "error");

            }
        }
    }
}
