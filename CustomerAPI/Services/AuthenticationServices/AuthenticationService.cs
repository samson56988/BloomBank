using AutoMapper;
using CustomerAPI.ApiServices;
using CustomerAPI.Dtos;
using CustomerAPI.Helpers;
using CustomerAPI.Models;
using DataBase.Models;
using Domain.CustomerDomains.Services.Interfaces;
using Domain.SharedModels;
using Library.Security;

namespace CustomerAPI.Services.AuthenticationServices
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IEncryptionService _encryptionService;
        private readonly IAccountService _aacountService;
        private readonly UserTokenGenerator _userToken;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMapper _mapper;
        ServiceResponse res = new ServiceResponse();
        string message = "";
        string error = "";

        public AuthenticationService(ICustomerRepository repository,IEncryptionService encryptionService,IAccountService accountService,UserTokenGenerator userTokenGenerator,IMapper mapper,IHttpContextAccessor httpContextAccessor) 
        { 
            _customerRepository = repository; 
            _encryptionService = encryptionService;
            _aacountService = accountService;
            _userToken = userTokenGenerator;
            _mapper = mapper;
            _contextAccessor = httpContextAccessor;
        }

        public async Task<Tuple<object, string>> CreateLoginDetails(CreateLoginDetails auth)
        {
            try
            {
                var request = _mapper.Map<UserLogin>(auth);

                if(request == null)
                {
                    message = "Error occured while creating your account";
                    res.success = false;
                    res.data = null;
                    res.message = message;
                    return new Tuple<object, string>(res, "error");
                }

                var getaccountdetails = await _aacountService.GetAccountDetailsAsync(request.AccountNo);

                if (getaccountdetails == null) 
                {
                    message = "Invalid Account Details";
                    res.success = false;
                    res.data = null;
                    res.message = message;
                    return new Tuple<object, string>(res, "error");
                }

                request.Password = _encryptionService.Encrypt(auth.Password);

                var customer = await _customerRepository.CreateUserLoginDetails(request);

                var activateCustomer = await _customerRepository.ActivateCustomer(request.AccountNo);

                if (customer)
                {
                    message = "Authentication Created Successfully";
                    res.success = true;
                    res.data = customer;
                    res.message = message;
                    return new Tuple<object, string>(res, "success");
                }
                else
                {
                    message = "Error occured while proccessing request";
                    res.success = false;
                    res.data = customer;
                    res.message = message;
                    return new Tuple<object, string>(res, "error");
                }


            }
            catch 
            {
                message = "Error occured while proccessing request";
                res.success = true;
                res.data = null;
                res.message = message;
                return new Tuple<object, string>(res, "error");
            }
        }

        public async Task<Tuple<object, string>> Login(CreateLoginDetails auth)
        {
            try
            {
                var request = _mapper.Map<UserLogin>(auth);

                if (request == null)
                {
                    message = "Something went wrong";
                    res.success = false;
                    res.data = null;
                    res.message = message;
                    return new Tuple<object, string>(res, "error");
                }

                request.Password = _encryptionService.Encrypt(auth.Password);

                var customer = await _customerRepository.UserLogin(request);

                if (customer.customer != null)
                {
                    UserTokenDetails tokenDetails = new UserTokenDetails();

                    tokenDetails.AccountNo = customer.customer.AccountNumber;
                    tokenDetails.Email = customer.customer.Email;
                    tokenDetails.UserId = Convert.ToString(customer.customer.Id);


                    var generateToken = _userToken.GenerateToken(tokenDetails);

                    if(generateToken == null)
                    {
                        message = "Something went wrong";
                        res.success = false;
                        res.data = null;
                        res.message = message;
                        return new Tuple<object, string>(res, "error");
                    }

                    message = generateToken;
                    res.success = true;
                    res.data = customer;
                    res.message = message;
                    return new Tuple<object, string>(res, "success");
                }
                else
                {
                    message = customer.message;
                    res.success = false;
                    res.data = customer;
                    res.message = message;
                    return new Tuple<object, string>(res, "error");
                }


            }
            catch
            {
                message = "Error occured while proccessing request";
                res.success = false;
                res.data = null;
                res.message = message;
                return new Tuple<object, string>(res, "error");
            }
        }
    }
}
