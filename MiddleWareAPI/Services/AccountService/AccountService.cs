using DataBase.Models;
using Domain.SharedModels;
using MiddleWareAPI.DatabaseLogic;
using MiddleWareAPI.Models.Dto;
using MiddleWareDomain.Models;
using System;
using System.Reflection.Metadata;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;

namespace MiddleWareAPI.Services.AccountService
{
    public class AccountService : IAccountService
    {
        private static readonly Random random = new Random();
        private readonly IDatabaseLogic _databaseLogic;
        ServiceResponse res = new ServiceResponse();
        private readonly ILogger<AccountService> _logger;
        private readonly IAccountConfiguration _accountConfiguration;
        string message = "";
        string error = "";


        public AccountService(IDatabaseLogic databaseLogic, ILogger<AccountService> logger, IAccountConfiguration accountConfiguration)
        {
            _databaseLogic = databaseLogic;
            _logger = logger;
            _accountConfiguration = accountConfiguration;
        }

        public async Task<Tuple<object, string>> CreateAccountService(CreateCustomerDto customer)
        {
            try
            {
                if(customer.HasBvn == false)
                {
                    customer.BVN = GenerateBVN(customer.DateOnboarded);
                }


                var customerrDetails = MapCreateCustomerRequest(customer);

                if(customerrDetails == null)
                {
                    message = "Invalid Request Data";   
                    res.success = false;
                    res.data = null;
                    res.message = message;
                    return new Tuple<object, string>(res, "error");
                }
                else
                {
                    //create account number 


                    customerrDetails.AccountNo = GenerateAccountNumber(customer.Phone.ToString());

                    if(customerrDetails.AccountNo == null)
                    {
                        message = "Error occured while creating account";
                        res.success = false;
                        res.data = null;
                        res.message = message;
                        return new Tuple<object, string>(res, "error");
                    }

                    var checkIfAccountExist = await _databaseLogic.GetCustomerByAccountNumber(customerrDetails.AccountNo);


                    if(checkIfAccountExist != null)
                    {
                        message = "Account already exist";
                        res.success = false;
                        res.data = null;
                        res.message = message;
                        return new Tuple<object, string>(res, "error");
                    }


                    var checkIfEmailExist =  await _databaseLogic.GetCustomerByEmail(customer.Email);

                    if(checkIfEmailExist != null)
                    {
                        message = "Account with email already exist";
                        res.success = false;
                        res.data = null;
                        res.message = message;
                        return new Tuple<object, string>(res, "error");
                    }

                    var checkIfMobileNoExist = await _databaseLogic.GetCustomerByPhoneNumber(customer.Phone);

                    if(checkIfMobileNoExist != null)
                    {
                        message = "Account with mobile already exist";
                        res.success = false;
                        res.data = null;
                        res.message = message;
                        return new Tuple<object, string>(res, "error");
                    }

                    var transactionLimit = _accountConfiguration.ConfigureAccount(customerrDetails.CurrencyCode, customerrDetails.AccountType);

                    if(transactionLimit == 0)
                    {
                        message = "Error occured while creating account";
                        res.success = false;
                        res.data = null;
                        res.message = message;
                        return new Tuple<object, string>(res, "error");
                    }

                    customerrDetails.TransactionLimit = transactionLimit;

                    var createCustomer = await _databaseLogic.CreateCustomer(customerrDetails);

                    if (createCustomer != null)
                    {
                        message = "Customer Created Successfully";
                        res.success = true;
                        res.data = createCustomer;
                        res.message = message;
                        return new Tuple<object, string>(res, "success");
                    }
                    else
                    {
                        message = "failed to create customer";
                        res.success = false;
                        res.data = null;
                        res.message = message;
                        return new Tuple<object, string>(res, "error");
                    }
                }
            }
            catch(Exception ex) 
            {
                message = "failed to create customer";
                _logger.LogError( $"{message} {ex.Message}");
                res.success = false;
                res.data = null;
                res.message = message;
                return new Tuple<object, string>(res, "error");
            }    
        }

        public MiddleWareDomain.Models.BloomCustomer MapCreateCustomerRequest(CreateCustomerDto dto)
        {
            return new BloomCustomer
            {
                FirstName = dto.FirstName,
                MiddleName = dto.MiddleName,
                LastName = dto.LastName,
                Email = dto.Email,
                Address = dto.Address,
                City = dto.City,
                Region = dto.Region,
                PostalCode = dto.PostalCode,
                Country = dto.Country,
                Phone = dto.Phone,
                AccountType = dto.AccountType,
                IdentificationType = dto.IdentificationType,
                IdentificationNumber = dto.IdNo,
                DateOnboarded = dto.DateOnboarded,
                DateOfBirth = dto.DOB,
                BVN = dto.BVN,
                TransactionLimit = 1000000,
                CifNo = GenerateCif(dto.Phone),
                CurrencyCode = dto.CurrencyCode,
                AccountName = dto.AccountName
            };
        }

        public static string GenerateBVN(DateTime dateOnboarded)
        {
            string firstDigits = DateTime.Now.ToString("MMdd");

            string secondDigits = dateOnboarded.ToString("MMdd");

            string randomDigits = random.Next(100, 999).ToString();

            return $"{firstDigits}{secondDigits}{randomDigits}";
        }

        public static string GenerateAccountNumber(string phoneNumber)
        {
            phoneNumber = phoneNumber.Trim();
            if (phoneNumber.Length > 10)
            {
                phoneNumber = phoneNumber.Substring(0, 10);
            }
            return phoneNumber;
        }

        public string GenerateCif(string phoneNumber)
        {
            string digitsOnly = Regex.Replace(phoneNumber, @"\D", "");

            if (digitsOnly.Length >= 4)
            {
                Random random = new Random();
                int startIndex = random.Next(0, digitsOnly.Length - 3);
                string randomDigits = digitsOnly.Substring(startIndex, 4);

                return randomDigits;
            }
            else
            {
                return "Phone number must contain at least 4 digits.";
            }
        }

        public async Task<Tuple<object, string>> GetAccountDetails(string accountNO)
        {
            try
            {
                var accountDetails = await _databaseLogic.GetCustomerByAccountNumber(accountNO);

                if(accountDetails == null)
                {
                    message = "Failed To Fetch Account Details";
                    res.success = false;
                    res.data = null;
                    res.message = message;
                    return new Tuple<object, string>(res, "error");
                }
                else
                {
                    message = "Account Details Fetched Successfully";
                    res.success = true;
                    res.data = accountDetails;
                    res.message = message;
                    return new Tuple<object, string>(res, "success");
                }
            }
            catch(Exception ex) 
            {
                message = "Failed To Fetch Account Details";
                _logger.LogError( $"{message} {ex.Message}");
                res.success = false;
                res.data = null;
                res.message = message;
                return new Tuple<object, string>(res, "error");
            }
        }

        public async Task<Tuple<object, string>> SetAccountOnPND(string AccountNO,bool hasPND)
        {
            try
            {
                var configurePND = await _databaseLogic.SetAccountOnPND(AccountNO, hasPND);

                if(configurePND == false)
                {
                    message = "Error processing request";
                    res.success = false;
                    res.data = null;
                    res.message = message;
                    return new Tuple<object, string>(res, "error");
                }
                else
                {
                    message = "Request successful";
                    res.success = true;
                    res.data = configurePND;
                    res.message = message;
                    return new Tuple<object, string>(res, "success");
                }
            }
            catch(Exception ex)
            {
                message = "Error processing request";
                _logger.LogError($"{message} {ex.Message}");
                res.success = false;
                res.data = null;
                res.message = message;
                return new Tuple<object, string>(res, "error");
            }
        }

        public async Task<Tuple<object, string>> CreateCorporateWalletAccount(CreateWalletAccountDto request)
        {
            try
            {
                var validateAccount = await _databaseLogic.GetCustomerByAccountNumber(request.AccountNumber);            

                if(validateAccount == null)
                {
                    message = "Account does not exist!";
                    res.success = false;
                    res.data = null;
                    res.message = message;
                    return new Tuple<object, string>(res, "error");
                }

                if( await _databaseLogic.GetCoporateAccountByBusinessName(request.BusinessName) != null)
                {
                    message = "Business Already Exist!";
                    res.success = false;
                    res.data = null;
                    res.message = message;
                    return new Tuple<object, string>(res, "error");
                }

                if(validateAccount.AccountType != "Corporate" || validateAccount.HasPND)
                {
                    message = "Account Provided Cannot Create Business";
                    res.success = false;
                    res.data = null;
                    res.message = message;
                    return new Tuple<object, string>(res, "error");
                }

                var corprateWallet = MapToCorprateWalletAccountSetup(request);

                var createAccount = await _databaseLogic.CreateCoporateWalletAccount(corprateWallet);

                if(createAccount == false)
                {
                    message = "Error processing request";
                    res.success = false;
                    res.data = null;
                    res.message = message;
                    return new Tuple<object, string>(res, "error");
                }
                else
                {
                    message = "Request Successful";
                    res.success = true;
                    res.data = corprateWallet;
                    res.message = message;
                    return new Tuple<object, string>(res, "success");
                }                   
            }
            catch(Exception ex)
            {
                message = "Error processing request";
                _logger.LogError($"{message} {ex.Message}");
                res.success = false;
                res.data = null;
                res.message = message;
                return new Tuple<object, string>(res, "error");
            }
        }

        public CorprateWalletAccountSetup MapToCorprateWalletAccountSetup(CreateWalletAccountDto dto)
        {
            return new CorprateWalletAccountSetup
            {
                AccountNumber = dto.AccountNumber,
                CurrencyCode = dto.CurrencyCode,
                AccountName = dto.AccountName,
                DateCreated = dto.DateCreated,
                DateDeactivated = dto.DateDeactivated,
                BusinessName = dto.BusinessName,
                IsDeactivated = dto.IsDeactivated,
                // Generate BusinessKey here
                BusinessKey = GenerateBusinessKey(dto.BusinessName)
            };
        }

        public static string GenerateBusinessKey(string businessName)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string randomChars = new string(Enumerable.Repeat(chars, 12)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            string businessKey = businessName.Replace(" ", "") + randomChars;

            return businessKey;
        }

        public async Task<Tuple<object, string>> GetCorporateWalletAccount(string businessKey)
        {
            try
            {
                var getDetails = await _databaseLogic.GetCoporateWalletAccountDetails(businessKey);

                if(getDetails == null)
                {
                    message = "Error processing request";
                    res.success = false;
                    res.data = null;
                    res.message = message;
                    return new Tuple<object, string>(res, "error");
                }
                else
                {
                    message = "Coporate account details fetched successfully";
                    res.success = true;
                    res.data = getDetails;
                    res.message = message;
                    return new Tuple<object, string>(res, "success");
                }
            }
            catch(Exception ex)
            {
                message = "Error processing request";
                _logger.LogError($"{message} {ex.Message}");
                res.success = false;
                res.data = null;
                res.message = message;
                return new Tuple<object, string>(res, "error");
            }
        }

        public async Task<Tuple<object, string>> DeactivateCorporateWalletAccount(string businessKey)
        {
            try
            {
                var deactivateCorporateWallet = await _databaseLogic.DeactivateCorporateWalletAccount(businessKey);

                if (deactivateCorporateWallet)
                {
                    message = $"Wallet Account Service Deactivated For {businessKey}";
                    res.success = true;
                    res.data = deactivateCorporateWallet;
                    res.message = message;
                    return new Tuple<object, string>(res, "success");
                }
                else
                {
                    message = "Error processing request";
                    res.success = false;
                    res.data = null;
                    res.message = message;
                    return new Tuple<object, string>(res, "error");
                }
            }
            catch(Exception ex)
            {
                message = "Error processing request";
                _logger.LogError($"{message} {ex.Message}");
                res.success = false;
                res.data = null;
                res.message = message;
                return new Tuple<object, string>(res, "error");
            }
        }

        public async Task<Tuple<object, string>> CreateWalletAccount(WalletAccountDto dto)
        {
            try
            {
                
                var getAccountDetails = await _databaseLogic.GetCoporateWalletAccountDetails(dto.CoprateAccountBusinessKey);

                if (getAccountDetails == null)
                {
                    message = $"This account does not have wallet business";
                    res.success = false;
                    res.data = getAccountDetails;
                    res.message = message;
                    return new Tuple<object, string>(res, "error");
                }


                if (getAccountDetails.IsDeactivated)
                {
                    message = $"Wallet Service Deactivated for {getAccountDetails.AccountNumber}";
                    res.success = false;
                    res.data = getAccountDetails;
                    res.message = message;
                    return new Tuple<object, string>(res, "error");
                }

                var account = MapToWalletAccount(dto);

                account.CurrencyCode = getAccountDetails.CurrencyCode;
                account.CoprateAccountName = getAccountDetails.AccountName;

                var createWallet = await _databaseLogic.CreateWalletAccount(account);
                if (createWallet)
                {
                    message = $"Wallet Account Created Successfully";
                    res.success = true;
                    res.data = account;
                    res.message = message;
                    return new Tuple<object, string>(res, "success");
                }
                else
                {
                    message = "Error processing request";
                    res.success = false;
                    res.data = null;
                    res.message = message;
                    return new Tuple<object, string>(res, "error");
                }
            }
            catch(Exception ex)
            {
                message = "Error processing request";
                _logger.LogError($"{message} {ex.Message}");
                res.success = false;
                res.data = null;
                res.message = message;
                return new Tuple<object, string>(res, "error");
            }
        }


        public static WalletAccount MapToWalletAccount(WalletAccountDto dto)
        {
            string walletNumber = DateTime.UtcNow.ToString("yyMMddHHmm") + GenerateRandomDigitsForWallet(1);
            return new WalletAccount
            {
                CoprateAccountNo = dto.CoprateAccountNo,
                AccountBalance = 0,
                CoprateAccountBusinessKey = dto.CoprateAccountBusinessKey,
                DateCreated = dto.DateCreated,
                WalletAccountName = dto.WalletAccountName,
                WalletNo = walletNumber
            };
        }

        public static VirtualCardDetails MapToVirtualCardDetails(VirtualCardDetailsDto dto)
        {
            string cardPan = DateTime.UtcNow.ToString("yyMMddHHmm") + GenerateRandomDigitsForWallet(8);

            return new VirtualCardDetails
            {
                CustomerId = dto.CustomerId,
                CardType = dto.CardType,
                CardProvider = dto.CardProvider,
                CardId = GenerateCardId(), // Generate a unique card id
                Cvv = GenerateCardCVV(),                     // Set other properties to default values or leave them as null
                CardName = null,
                CardPAN = cardPan,
                CardExpiryDate = GenerateCardExpiryDate(),
                CardPin = null,
                CardStatus = null,
                IsActive = false
            };
        }

        public static string GenerateCardId()
        {
            // Generate a unique card id logic here
            return Guid.NewGuid().ToString("N").Substring(0, 16);
        }

        public static string GenerateCardCVV()
        {
            // Generate card CVV logic here
            // Example: Generate a random 3-digit number
            Random rand = new Random();
            return rand.Next(100, 999).ToString();
        }

        public static string GenerateCardExpiryDate()
        {
            return DateTime.UtcNow.AddYears(2).ToString("MM/yy");
        }

       

        private static string GenerateRandomDigitsForWallet(int length)
        {
            Random random = new Random();
            StringBuilder builder = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                builder.Append(random.Next(10)); 
            }
            return builder.ToString();
        }

        public async Task<Tuple<object, string>> CreateVirtualCard(VirtualCardDetailsDto account)
        {
            try
            {
                var getAccountDetails = await _databaseLogic.GetCustomerByAccountNumber(account.CustomerId);

                if(getAccountDetails == null)
                {
                    message = "Invalid Account Provided";
                    res.success = false;
                    res.data = null;
                    res.message = message;
                    return new Tuple<object, string>(res, "error");
                }

                var activeCard = await _databaseLogic.GetCustomerActiveCardDetails(account.CustomerId);

                if(activeCard != null)
                {
                    if(activeCard.Any(x => x.CardType == account.CardType))
                    {
                        message = $"Customer already has an active {account.CardType} card";
                        res.success = false;
                        res.data = null;
                        res.message = message;
                        return new Tuple<object, string>(res, "error");
                    }
                }

                var createCard = MapToVirtualCardDetails(account);

                createCard.CardStatus = "InActive";

                createCard.CardName = getAccountDetails.AccountName;

                var createWallet = await _databaseLogic.CreateVirtualCard(createCard);
                if (createWallet)
                {
                    message = $"Virtual Account Created Successfully";
                    res.success = true;
                    res.data = account;
                    res.message = message;
                    return new Tuple<object, string>(res, "success");
                }
                else
                {
                    message = "Error processing request";
                    res.success = false;
                    res.data = null;
                    res.message = message;
                    return new Tuple<object, string>(res, "error");
                }
            }
            catch(Exception ex)
            {
                message = "Error processing request";
                _logger.LogError($"{message} {ex.Message}");
                res.success = false;
                res.data = null;
                res.message = message;
                return new Tuple<object, string>(res, "error");
            }
        }

        public async Task<Tuple<object, string>> BlockVirtualCard(string customerId, string cardId)
        {
            try
            {
                var blockCard = await _databaseLogic.BlockVirtualCard(customerId,cardId);

                if(blockCard)
                {
                    message = $"Virtual Account Blocked Successfully";
                    res.success = true;
                    res.data = blockCard;
                    res.message = message;
                    return new Tuple<object, string>(res, "success");
                }
                else
                {
                    message = "Error processing request";
                    res.success = false;
                    res.data = null;
                    res.message = message;
                    return new Tuple<object, string>(res, "error");
                }
            }
            catch (Exception ex)
            {
                message = "Error processing request";
                _logger.LogError($"{message} {ex.Message}");
                res.success = false;
                res.data = null;
                res.message = message;
                return new Tuple<object, string>(res, "error");
            }
        }

        public async Task<Tuple<object, string>> GetCustomerActiveCards(string customerId)
        {
            try
            {
                var getActiveCard = await _databaseLogic.GetCustomerActiveCardDetails(customerId);
                if(getActiveCard == null)
                {
                    message = "No Active Card ";
                    res.success = false;
                    res.data = null;
                    res.message = message;
                    return new Tuple<object, string>(res, "error");
                }
                else
                {
                    message = "Active Cards Fetched";
                    res.success = true;
                    res.data = getActiveCard;
                    res.message = message;
                    return new Tuple<object, string>(res, "success");
                }

            }
            catch (Exception ex)
            {
                message = "Error processing request";
                _logger.LogError($"{message} {ex.Message}");
                res.success = false;
                res.data = null;
                res.message = message;
                return new Tuple<object, string>(res, "error");
            }
        }

        public async Task<Tuple<object, string>> ActivateVirtualCard(string customerId, string cardPin , string cardId)
        {
            try
            {
                var validateCard = await _databaseLogic.GetCardById(customerId);

                if(validateCard == null)
                {
                    message = "Error processing request";
                    res.success = false;
                    res.data = null;
                    res.message = message;
                    return new Tuple<object, string>(res, "error");
                }


                var activate = await _databaseLogic.ActivateNewCard(customerId, cardPin, cardId);
                if (!activate)
                {
                    message = "Error processing request";
                    res.success = false;
                    res.data = null;
                    res.message = message;
                    return new Tuple<object, string>(res, "error");
                }
                else
                {
                    message = "Activated Successfully";
                    res.success = true;
                    res.data = activate;
                    res.message = message;
                    return new Tuple<object, string>(res, "success");
                }
            }
            catch (Exception ex)
            {
                message = "Error processing request";
                _logger.LogError($"{message} {ex.Message}");
                res.success = false;
                res.data = null;
                res.message = message;
                return new Tuple<object, string>(res, "error");
            }
        }

        public async Task<Tuple<object, string>> ActivateCorporateWalletAccount(string businessKey)
        {
            try
            {
                var deactivateCorporateWallet = await _databaseLogic.ActivateCorporateWalletAccount(businessKey);

                if (deactivateCorporateWallet)
                {
                    message = $"Wallet Account Service Activated For {businessKey}";
                    res.success = true;
                    res.data = deactivateCorporateWallet;
                    res.message = message;
                    return new Tuple<object, string>(res, "success");
                }
                else
                {
                    message = "Error processing request";
                    res.success = false;
                    res.data = null;
                    res.message = message;
                    return new Tuple<object, string>(res, "error");
                }
            }
            catch (Exception ex)
            {
                message = "Error processing request";
                _logger.LogError($"{message} {ex.Message}");
                res.success = false;
                res.data = null;
                res.message = message;
                return new Tuple<object, string>(res, "error");
            }
        }

        public async Task<Tuple<object, string>> FundAccount(string customerId, decimal amount)
        {
            try
            {
                var accountDetails = await _databaseLogic.GetCustomerByAccountNumber(customerId);

                if (accountDetails == null)
                {
                    message = "Account Does not Exist";
                    res.success = false;
                    res.data = null;
                    res.message = message;
                    return new Tuple<object, string>(res, "error");
                }

                var activate = await _databaseLogic.FundAccount(customerId, amount);
                if (!activate)
                {
                    message = "Error processing request";
                    res.success = false;
                    res.data = null;
                    res.message = message;
                    return new Tuple<object, string>(res, "error");
                }
                else
                {
                    message = "Funded Successfully";
                    res.success = true;
                    res.data = activate;
                    res.message = message;
                    return new Tuple<object, string>(res, "success");
                }
            }
            catch (Exception ex)
            {
                message = "Error processing request";
                _logger.LogError($"{message} {ex.Message}");
                res.success = false;
                res.data = null;
                res.message = message;
                return new Tuple<object, string>(res, "error");
            }
        }
    }
}
