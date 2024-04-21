using EmailProcessor.EmailServices;
using MiddleWareDomain.Models;
using Npgsql;
using NpgsqlTypes;
using Publisher;
using RabbitMQ.Client;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Text.Json;

namespace MiddleWareAPI.DatabaseLogic
{
    public interface IDatabaseLogic
    {
        //Account  

        Task<BloomCustomer> CreateCustomer(BloomCustomer customer);
        Task<BloomCustomer> GetCustomerByAccountNumber(string accountNumber);
        Task<bool> SetAccountOnPND(string accountNumber, bool hasPND);
        Task<bool> CreateCoporateWalletAccount(CorprateWalletAccountSetup accountSetup);
        Task<CorprateWalletAccountSetup> GetCoporateWalletAccountDetails(string businessKey);
        Task<bool> DeactivateCorporateWalletAccount(string accountNumber);
        Task<bool> CreateWalletAccount(WalletAccount account);
        Task<bool> CreateVirtualCard(VirtualCardDetails cardDetails);
        Task<bool> BlockVirtualCard(string customerId, string cardId);
        Task<bool> ActivateNewCard(string customerId, string pin, string Id);
        Task<List<VirtualCardDetails>> GetCustomerActiveCardDetails(string customerId);
        Task<BloomCustomer> GetCustomerByEmail(string accountNumber);
        Task<BloomCustomer> GetCustomerByPhoneNumber(string phoneNumber);
        Task<CorprateWalletAccountSetup> GetCoporateAccountByBusinessName(string businessName);
        Task<bool> ActivateCorporateWalletAccount(string accountNumber);
        Task<VirtualCardDetails> GetCardById(string cardId);
        Task<bool> FundAccount(string accountNumber, decimal amountToAdd);
        Task<decimal> GetTotalSuccessfulTransactionAmountForDay(string accountNumber, DateTime date);
        Task<List<BloomCustomer>> GetCustomers();

        //Transfer Service
        Task<bool> InitiateBankTransfer(BankTransfers customer);
        Task<bool> DebitCustomer(string senderAccount, string beneficiaryAccountNo, decimal amount, string Narration);

    }


    public class DatabaseLogic : IDatabaseLogic
    {
        public readonly IRabbitMQPublisher _rabbitMQPublisher;
        public readonly IEmailService _emailService;

        public string connectionString { get; }

        public DatabaseLogic(IRabbitMQPublisher rabbitMQPublisher, IEmailService emailService) 
        {
            connectionString = ConfigurationManager.AppSettings["DatabaseConnection"];
            _rabbitMQPublisher = rabbitMQPublisher;
            _emailService = emailService;
        }
   
        public async Task<BloomCustomer> CreateCustomer(BloomCustomer customer)
        {
            try
            {

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = @"
                INSERT INTO public.""BloomCustomers"" (""AccountNo"", ""FirstName"", ""MiddleName"", ""LastName"", ""Email"", ""Address"", ""City"", ""Region"", ""PostalCode"", ""Country"", ""Phone"", ""BVN"", ""AccountType"", ""IdentificationType"", ""IdentificationNumber"", ""TransactionLimit"", ""DateOnboarded"", ""AccountBalance"", ""CifNo"", ""DateOfBirth"", ""HasPND"", ""CurrencyCode"", ""AccountName"")
                    VALUES (@AccountNo, @FirstName, @MiddleName, @LastName, @Email, @Address, @City, @Region, @PostalCode, @Country, @Phone, @BVN, @AccountType, @IdentificationType, @IdentificationNumber, @TransactionLimit, @DateOnboarded, @AccountBalance, @CifNo, @DateOfBirth, @HasPND, @CurrencyCode, @AccountName);
                    ";

                        command.Parameters.AddWithValue("@AccountNo", customer.AccountNo);
                        command.Parameters.AddWithValue("@FirstName", customer.FirstName);
                        command.Parameters.AddWithValue("@MiddleName", customer.MiddleName);
                        command.Parameters.AddWithValue("@LastName", customer.LastName);
                        command.Parameters.AddWithValue("@Email", customer.Email);
                        command.Parameters.AddWithValue("@Address", customer.Address);
                        command.Parameters.AddWithValue("@City", customer.City);
                        command.Parameters.AddWithValue("@Region", customer.Region);
                        command.Parameters.AddWithValue("@PostalCode", customer.PostalCode);
                        command.Parameters.AddWithValue("@Country", customer.Country);
                        command.Parameters.AddWithValue("@Phone", customer.Phone);
                        command.Parameters.AddWithValue("@BVN", customer.BVN);
                        command.Parameters.AddWithValue("@AccountType", customer.AccountType);
                        command.Parameters.AddWithValue("@IdentificationType", customer.IdentificationType);
                        command.Parameters.AddWithValue("@IdentificationNumber", customer.IdentificationNumber);
                        command.Parameters.AddWithValue("@TransactionLimit", customer.TransactionLimit);
                        command.Parameters.AddWithValue("@DateOnboarded", customer.DateOnboarded);
                        command.Parameters.AddWithValue("@AccountBalance", customer.AccountBalance);
                        command.Parameters.AddWithValue("@CifNo", customer.CifNo);
                        command.Parameters.AddWithValue("@DateOfBirth", customer.DateOfBirth);
                        command.Parameters.AddWithValue("@HasPND", customer.HasPND);
                        command.Parameters.AddWithValue("@CurrencyCode", customer.CurrencyCode);
                        command.Parameters.AddWithValue("@AccountName", customer.AccountName);

                        int rowsAffected = await command.ExecuteNonQueryAsync();

                        await _emailService.AccountCreationNotification(customer.FirstName, customer.Email, customer.AccountNo, "Bloom Bank");
                        return customer;
                    }
                }
            }
            catch 
            {
                throw;
            }
        }

        public async Task<bool> FundAccount(string accountNumber, decimal amountToAdd)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = @"
                    UPDATE public.""BloomCustomers""
                    SET ""AccountBalance"" = ""AccountBalance"" + @AmountToAdd
                    WHERE ""AccountNo"" = @AccountNumber";

                        command.Parameters.AddWithValue("@AmountToAdd", amountToAdd);
                        command.Parameters.AddWithValue("@AccountNumber", accountNumber);

                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<BloomCustomer> GetCustomerByAccountNumber(string accountNumber)
        {
            BloomCustomer customer = null;

            try
            {

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = @"
                    SELECT ""AccountNo"", ""FirstName"", ""MiddleName"", ""LastName"", ""Email"", ""Address"", ""City"", ""Region"", ""PostalCode"", ""Country"", ""Phone"", ""BVN"", ""AccountType"", ""IdentificationType"", ""IdentificationNumber"",""AccountNo"", ""TransactionLimit"", ""DateOnboarded"", ""AccountBalance"", ""CifNo"", ""DateOfBirth"", ""HasPND"", ""CurrencyCode"",""AccountName""
                    FROM public.""BloomCustomers""
                    WHERE ""AccountNo"" = @AccountNumber";

                        command.Parameters.AddWithValue("@AccountNumber", accountNumber);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                customer = new BloomCustomer
                                {
                                    AccountNo = reader.GetString(reader.GetOrdinal("AccountNo")),
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    MiddleName = reader.GetString(reader.GetOrdinal("MiddleName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    Address = reader.GetString(reader.GetOrdinal("Address")),
                                    City = reader.GetString(reader.GetOrdinal("City")),
                                    Region = reader.GetString(reader.GetOrdinal("Region")),
                                    PostalCode = reader.GetString(reader.GetOrdinal("PostalCode")),
                                    Country = reader.GetString(reader.GetOrdinal("Country")),
                                    Phone = reader.GetString(reader.GetOrdinal("Phone")),
                                    BVN = reader.GetString(reader.GetOrdinal("BVN")),
                                    AccountType = reader.GetString(reader.GetOrdinal("AccountType")),
                                    IdentificationType = reader.GetString(reader.GetOrdinal("IdentificationType")),
                                    IdentificationNumber = reader.GetString(reader.GetOrdinal("IdentificationNumber")),
                                    TransactionLimit = reader.GetDecimal(reader.GetOrdinal("TransactionLimit")),
                                    DateOnboarded = reader.GetDateTime(reader.GetOrdinal("DateOnboarded")),
                                    AccountBalance = reader.GetDecimal(reader.GetOrdinal("AccountBalance")),
                                    CifNo = reader.GetString(reader.GetOrdinal("CifNo")),
                                    DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                                    HasPND = reader.GetBoolean(reader.GetOrdinal("HasPND")),
                                    CurrencyCode = reader.GetString(reader.GetOrdinal("CurrencyCode")),
                                    AccountName = reader.GetString(reader.GetOrdinal("AccountName"))
                                };
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }

            return customer;
        }

        public async Task<bool> SetAccountOnPND(string accountNumber, bool hasPND)
        {
            try
            {

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = @"
                    UPDATE public.""BloomCustomers""
                    SET ""HasPND"" = @HasPND
                    WHERE ""AccountNo"" = @AccountNumber";

                        command.Parameters.AddWithValue("@HasPND", hasPND);
                        command.Parameters.AddWithValue("@AccountNumber", accountNumber);

                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> CreateCoporateWalletAccount(CorprateWalletAccountSetup accountSetup)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string commandText = @"INSERT INTO public.""CorprateWalletAccountSetups""
                       (""AccountNumber"", ""BusinessKey"", ""CurrencyCode"", ""AccountName"", ""DateCreated"", ""DateDeactivated"", ""BusinessName"",""IsDeactivated"")
                       VALUES (@AccountNumber, @BusinessKey, @CurrencyCode, @AccountName, @DateCreated, @DateDeactivated, @BusinessName,@IsDeactivated)";

                    using (var command = new NpgsqlCommand(commandText, connection))
                    {
                        command.Parameters.AddWithValue("@AccountNumber", accountSetup.AccountNumber);
                        command.Parameters.AddWithValue("@BusinessKey", accountSetup.BusinessKey);
                        command.Parameters.AddWithValue("@CurrencyCode", accountSetup.CurrencyCode);
                        command.Parameters.AddWithValue("@AccountName", accountSetup.AccountName);
                        command.Parameters.AddWithValue("@DateCreated", accountSetup.DateCreated);
                        command.Parameters.AddWithValue("@DateDeactivated", accountSetup.DateDeactivated);
                        command.Parameters.AddWithValue("@BusinessName", accountSetup.BusinessName);
                        command.Parameters.AddWithValue("@IsDeactivated", accountSetup.IsDeactivated);

                        int rowsAffected = await command.ExecuteNonQueryAsync();

                        return rowsAffected > 0;
                    }
                }
            }
            catch 
            {
                throw;
            }
        }

        public async Task<CorprateWalletAccountSetup> GetCoporateWalletAccountDetails(string businessKey)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string commandText = @"SELECT * FROM public.""CorprateWalletAccountSetups"" WHERE ""BusinessKey"" = @BusinessKey";

                    using (var command = new NpgsqlCommand(commandText, connection))
                    {
                        command.Parameters.AddWithValue("@BusinessKey", businessKey);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return new CorprateWalletAccountSetup
                                {
                                    AccountNumber = reader.GetString(reader.GetOrdinal("AccountNumber")),
                                    BusinessKey = reader.GetString(reader.GetOrdinal("BusinessKey")),
                                    CurrencyCode = reader.GetString(reader.GetOrdinal("CurrencyCode")),
                                    AccountName = reader.GetString(reader.GetOrdinal("AccountName")),
                                    DateCreated = reader.GetDateTime(reader.GetOrdinal("DateCreated")),
                                    BusinessName = reader.GetString(reader.GetOrdinal("BusinessName")),
                                    IsDeactivated = reader.GetBoolean(reader.GetOrdinal("IsDeactivated"))
                                };
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                }
            }
            catch
            {
                throw; 
            }
        }

        public async Task<CorprateWalletAccountSetup> GetCoporateAccountByBusinessName(string businessName)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string commandText = @"SELECT * FROM ""CorprateWalletAccountSetups"" WHERE ""BusinessName"" = @BusinessName";

                    using (var command = new NpgsqlCommand(commandText, connection))
                    {
                        command.Parameters.AddWithValue("@BusinessName", businessName);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return new CorprateWalletAccountSetup
                                {
                                    AccountNumber = reader.GetString(reader.GetOrdinal("AccountNumber")),
                                    BusinessKey = reader.GetString(reader.GetOrdinal("BusinessKey")),
                                    CurrencyCode = reader.GetString(reader.GetOrdinal("CurrencyCode")),
                                    AccountName = reader.GetString(reader.GetOrdinal("AccountName")),
                                    DateCreated = reader.GetDateTime(reader.GetOrdinal("DateCreated")),
                                    DateDeactivated = !reader.IsDBNull(reader.GetOrdinal("DateDeactivated")) ? reader.GetDateTime(reader.GetOrdinal("DateDeactivated")) : default(DateTime),
                                    BusinessName = reader.GetString(reader.GetOrdinal("BusinessName")),
                                    IsDeactivated = reader.GetBoolean(reader.GetOrdinal("IsDeactivated"))
                                };
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> ActivateCorporateWalletAccount(string accountNumber)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = @"
                    UPDATE ""CorprateWalletAccountSetups"" 
                    SET ""IsDeactivated"" = false, ""DateDeactivated"" = NULL 
                    WHERE ""BusinessKey"" = @AccountNumber";

                        command.Parameters.AddWithValue("@AccountNumber", accountNumber);

                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> DeactivateCorporateWalletAccount(string accountNumber)
        {
            try
            {

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = @"
                        UPDATE ""CorprateWalletAccountSetups"" 
                        SET ""IsDeactivated"" = true, ""DateDeactivated"" = @DateDeactivated 
                        WHERE ""BusinessKey"" = @AccountNumber";

                        command.Parameters.AddWithValue("@AccountNumber", accountNumber);
                        command.Parameters.AddWithValue("@DateDeactivated", DateTime.UtcNow);

                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> CreateWalletAccount(WalletAccount account)
        {
            try
            {

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = @"
                        INSERT INTO ""WalletAccount"" (""WalletNo"", ""CoprateAccountNo"", ""CoprateAccountName"", ""AccountBalance"", ""CoprateAccountBusinessKey"", ""DateCreated"", ""CurrencyCode"", ""WalletAccountName"")
                  VALUES (@WalletNo, @CoprateAccountNo, @CoprateAccountName, @AccountBalance, @CoprateAccountBusinessKey, @DateCreated, @CurrencyCode, @WalletAccountName)";

                        command.Parameters.AddWithValue("@WalletNo", account.WalletNo);
                        command.Parameters.AddWithValue("@CoprateAccountNo", account.CoprateAccountNo);
                        command.Parameters.AddWithValue("@CoprateAccountName", account.CoprateAccountName);
                        command.Parameters.AddWithValue("@AccountBalance", account.AccountBalance);
                        command.Parameters.AddWithValue("@CoprateAccountBusinessKey", account.CoprateAccountBusinessKey);
                        command.Parameters.AddWithValue("@DateCreated", account.DateCreated);
                        command.Parameters.AddWithValue("@CurrencyCode", account.CurrencyCode);
                        command.Parameters.AddWithValue("@WalletAccountName", account.WalletAccountName);

                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> CreateVirtualCard(VirtualCardDetails cardDetails)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = @"
                    INSERT INTO ""VirtualCardDetails"" (""CardId"", ""CustomerId"", ""CardName"", ""CardPAN"", ""CardType"", ""CardProvider"", ""CardExpiryDate"", ""CardPin"", ""CardStatus"", ""IsActive"",""Cvv"")
                    VALUES (@CardId, @CustomerId, @CardName, @CardPAN, @CardType, @CardProvider, @CardExpiryDate, @CardPin, @CardStatus, @IsActive, @Cvv)";


                        command.Parameters.AddWithValue("@CardId", cardDetails.CardId);
                        command.Parameters.AddWithValue("@CardName", cardDetails.CardName);
                        command.Parameters.AddWithValue("@CardPAN", cardDetails.CardPAN);
                        command.Parameters.AddWithValue("@CardType", cardDetails.CardType);
                        command.Parameters.AddWithValue("@CardProvider", cardDetails.CardProvider);
                        command.Parameters.AddWithValue("@CardExpiryDate", cardDetails.CardExpiryDate);
                        command.Parameters.AddWithValue("@CardPin", "No Pin");
                        command.Parameters.AddWithValue("@CardStatus", cardDetails.CardStatus);
                        command.Parameters.AddWithValue("@IsActive", false);
                        command.Parameters.AddWithValue("@CustomerId", cardDetails.CustomerId);
                        command.Parameters.AddWithValue("@Cvv", cardDetails.Cvv);

                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<VirtualCardDetails> GetCardById(string cardId)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = @"
                    SELECT * 
                    FROM ""VirtualCardDetails"" 
                    WHERE ""CardId"" = @CardId";

                        command.Parameters.AddWithValue("@CardId", cardId);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return new VirtualCardDetails
                                {
                                    CardId = reader.GetString(reader.GetOrdinal("CardId")),
                                    CustomerId = reader.GetString(reader.GetOrdinal("CustomerId")),
                                    CardName = reader.GetString(reader.GetOrdinal("CardName")),
                                    CardPAN = reader.GetString(reader.GetOrdinal("CardPAN")),
                                    CardType = reader.GetString(reader.GetOrdinal("CardType")),
                                    CardProvider = reader.GetString(reader.GetOrdinal("CardProvider")),
                                    CardExpiryDate = reader.GetString(reader.GetOrdinal("CardExpiryDate")),
                                    CardPin = reader.GetString(reader.GetOrdinal("CardPin")),
                                    CardStatus = reader.GetString(reader.GetOrdinal("CardStatus")),
                                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                    Cvv = reader.GetString(reader.GetOrdinal("Cvv"))
                                };
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> BlockVirtualCard(string customerId, string cardId)
        {
            try
            {

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = @"
                        UPDATE ""VirtualCardDetails""
                        SET ""CardStatus"" = @CardStatus, ""IsActive"" = false
                        WHERE ""CustomerId"" = @CustomerId AND ""CardId"" = @CardId";

                        command.Parameters.AddWithValue("@CardStatus", "Blocked");
                        command.Parameters.AddWithValue("@CustomerId", customerId);
                        command.Parameters.AddWithValue("@CardId", cardId);
                        int rowsAffected = await command.ExecuteNonQueryAsync();

                        return rowsAffected > 0;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> ActivateNewCard(string Id, string pin, string customerId)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = @"
                        UPDATE ""VirtualCardDetails""
                        SET ""CardStatus"" = @CardStatus,
                            ""CardPin"" = @CardPin,
                            ""IsActive"" = @IsActive
                        WHERE ""CustomerId"" = @CustomerId AND ""CardId"" = @CardId";


                        command.Parameters.AddWithValue("@CardStatus", "Active");
                        command.Parameters.AddWithValue("@CardPin", pin);
                        command.Parameters.AddWithValue("@IsActive", true);
                        command.Parameters.AddWithValue("@CustomerId", customerId);
                        command.Parameters.AddWithValue("@CardId", Id);

                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<VirtualCardDetails>> GetCustomerActiveCardDetails(string customerId)
        {
            try
            {
                var activeCards = new List<VirtualCardDetails>();

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = @"
                            SELECT * 
                            FROM ""VirtualCardDetails""
                            WHERE ""CustomerId"" = @CustomerId AND ""IsActive"" = true";

                        command.Parameters.AddWithValue("@CustomerId", customerId);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var card = new VirtualCardDetails
                                {
                                    CardId = reader["CardId"].ToString(),
                                    CardName = reader["CardName"].ToString(),
                                    CardPAN = reader["CardPAN"].ToString(),
                                    CardType = reader["CardType"].ToString(),
                                    CardProvider = reader["CardProvider"].ToString(),
                                    CardExpiryDate = reader["CardExpiryDate"].ToString(),
                                    CardPin = reader["CardPin"].ToString(),
                                    CardStatus = reader["CardStatus"].ToString(),
                                    IsActive = Convert.ToBoolean(reader["IsActive"])
                                };

                                activeCards.Add(card);
                            }
                        }
                    }
                }

                return activeCards;
            }
            catch
            {
                throw; 
            }
        }

        public async Task<bool> InitiateBankTransfer(BankTransfers transfer)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = @"
                        INSERT INTO ""BankTransfer"" (
                            ""Id"", 
                            ""TransactionReference"", 
                            ""SenderName"", 
                            ""SenderAccountNumber"", 
                            ""PayeeAccountNumber"", 
                            ""PayeeName"", 
                            ""TransferDate"", 
                            ""IsSuccessful"", 
                            ""TransferStatus"", 
                            ""Narration"", 
                            ""StatusFlag"",  
                            ""PlatForm"",
                            ""Amount""
                            
                        ) VALUES (
                            @Id, 
                            @TransactionReference, 
                            @SenderName, 
                            @SenderAccountNumber, 
                            @PayeeAccountNumber, 
                            @PayeeName, 
                            @TransferDate, 
                            @IsSuccessful, 
                            @TransferStatus, 
                            @Narration, 
                            @StatusFlag, 
                            @PlatForm,
                            @Amount
                            
                        )";

                        command.Parameters.AddWithValue("@Id", Guid.NewGuid());
                        command.Parameters.AddWithValue("@TransactionReference", transfer.TransactionReference);
                        command.Parameters.AddWithValue("@SenderName", transfer.SenderName);
                        command.Parameters.AddWithValue("@SenderAccountNumber", transfer.SenderAccountNumber);
                        command.Parameters.AddWithValue("@PayeeAccountNumber", transfer.PayeeAccountNumber);
                        command.Parameters.AddWithValue("@PayeeName", transfer.PayeeName);
                        command.Parameters.AddWithValue("@TransferDate", DateTime.UtcNow);
                        command.Parameters.AddWithValue("@IsSuccessful", transfer.IsSuccessful);
                        command.Parameters.AddWithValue("@TransferStatus", "Pending");
                        command.Parameters.AddWithValue("@Narration", transfer.Narration);
                        command.Parameters.AddWithValue("@StatusFlag", "0");
                        command.Parameters.AddWithValue("@PlatForm", transfer.PlatForm);
                        command.Parameters.AddWithValue("@Amount", transfer.Amount);

                        int rowsAffected = await command.ExecuteNonQueryAsync();

                        string message = JsonSerializer.Serialize(transfer);

                        //Send to Que

                        var submit = _rabbitMQPublisher.PublishBankTransfer("bank_transfer_queue", message);
                       
                        return rowsAffected > 0;


                        
                    }
                }       
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> DebitCustomer(string senderAccount, string beneficiaryAccountNo, decimal amount, string Narration)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    // Start a transaction
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // Retrieve sender's account details
                            var senderAccountDetails = await GetAccountDetails(senderAccount, connection, transaction);
                            if (senderAccountDetails == null)
                            {

                                //Audit Trail this when you rebuild
                                await _emailService.BankTransferNotification(senderAccountDetails.FirstName, senderAccountDetails.Email, DateTime.Now.ToString("MM-dd-yyyy"), "Bank Transfer", Narration, amount.ToString(), false);

                                //throw new InvalidOperationException($"Sender account '{senderAccount}' not found.");

                                return false;
                            }

                            // Check if sender has sufficient balance
                            if (senderAccountDetails.AccountBalance < amount)
                            {
                                await _emailService.BankTransferNotification(senderAccountDetails.FirstName, senderAccountDetails.Email, DateTime.Now.ToString("MM-dd-yyyy"), "Bank Transfer", Narration, amount.ToString(), false);

                                //throw new InvalidOperationException($"Insufficient balance in sender account '{senderAccount}'.");

                                return false;
                            }

                            // Update sender's account balance
                            senderAccountDetails.AccountBalance -= amount;
                            await UpdateAccountBalance(senderAccountDetails, connection, transaction);

                            // Retrieve beneficiary's account details
                            var beneficiaryAccountDetails = await GetAccountDetails(beneficiaryAccountNo, connection, transaction);
                            if (beneficiaryAccountDetails == null)
                            {
                                await _emailService.BankTransferNotification(senderAccountDetails.FirstName, senderAccountDetails.Email, DateTime.Now.ToString("MM-dd-yyyy"), "Bank Transfer", Narration, amount.ToString(), false);

                                //throw new InvalidOperationException($"Beneficiary account '{beneficiaryAccountNo}' not found.");

                                return false;
                            }

                            // Update beneficiary's account balance
                            beneficiaryAccountDetails.AccountBalance += amount;
                            await UpdateAccountBalance(beneficiaryAccountDetails, connection, transaction);

                            // Commit the transaction
                            transaction.Commit();

                            var sendEmail = await _emailService.BankTransferNotification(senderAccountDetails.FirstName, senderAccountDetails.Email, DateTime.Now.ToString("MM-dd-yyyy"), "Debit", Narration, amount.ToString(), true);

                            return true;
                        }
                        catch 
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch 
            {
                throw;
            }
        }

        private async Task<BloomCustomer> GetAccountDetails(string accountNumber, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            try
            {
                using (var command = connection.CreateCommand())
                {
                    command.Transaction = transaction;
                    command.CommandText = @"
                        SELECT * FROM ""BloomCustomers""
                        WHERE ""AccountNo"" = @AccountNumber";
                    command.Parameters.AddWithValue("@AccountNumber", accountNumber);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new BloomCustomer
                            {
                                AccountNo = reader.GetString(reader.GetOrdinal("AccountNo")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                MiddleName = reader.GetString(reader.GetOrdinal("MiddleName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Address = reader.GetString(reader.GetOrdinal("Address")),
                                City = reader.GetString(reader.GetOrdinal("City")),
                                Region = reader.GetString(reader.GetOrdinal("Region")),
                                PostalCode = reader.GetString(reader.GetOrdinal("PostalCode")),
                                Country = reader.GetString(reader.GetOrdinal("Country")),
                                Phone = reader.GetString(reader.GetOrdinal("Phone")),
                                BVN = reader.GetString(reader.GetOrdinal("BVN")),
                                AccountType = reader.GetString(reader.GetOrdinal("AccountType")),
                                IdentificationType = reader.GetString(reader.GetOrdinal("IdentificationType")),
                                IdentificationNumber = reader.GetString(reader.GetOrdinal("IdentificationNumber")),
                                TransactionLimit = reader.GetDecimal(reader.GetOrdinal("TransactionLimit")),
                                DateOnboarded = reader.GetDateTime(reader.GetOrdinal("DateOnboarded")),
                                AccountBalance = reader.GetDecimal(reader.GetOrdinal("AccountBalance")),
                                CifNo = reader.GetString(reader.GetOrdinal("CifNo")),
                                DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                                HasPND = reader.GetBoolean(reader.GetOrdinal("HasPND")),
                                CurrencyCode = reader.GetString(reader.GetOrdinal("CurrencyCode"))
                            };
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }

        }

        private async Task UpdateAccountBalance(BloomCustomer accountDetails, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            try
            {
                using (var command = connection.CreateCommand())
                {
                    command.Transaction = transaction;
                    command.CommandText = @"
                        UPDATE ""BloomCustomers""
                        SET ""AccountBalance"" = @AccountBalance
                        WHERE ""AccountNo"" = @AccountNo";

                    command.Parameters.AddWithValue("@AccountBalance", accountDetails.AccountBalance);
                    command.Parameters.AddWithValue("@AccountNo", accountDetails.AccountNo);

                    await command.ExecuteNonQueryAsync();
                }
            }
            catch
            {
                throw;
            }
            
        }

        public async Task<BloomCustomer> GetCustomerByEmail(string email)
        {
            BloomCustomer customer = null;

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = @"
                    SELECT ""AccountNo"", ""FirstName"", ""MiddleName"", ""LastName"", ""Email"", ""Address"", ""City"", ""Region"", ""PostalCode"", ""Country"", ""Phone"", ""BVN"", ""AccountType"", ""IdentificationType"", ""IdentificationNumber"", ""TransactionLimit"", ""DateOnboarded"", ""AccountBalance"", ""CifNo"", ""DateOfBirth"", ""HasPND"", ""CurrencyCode""
                    FROM public.""BloomCustomers""
                    WHERE ""Email"" = @Email";

                        command.Parameters.AddWithValue("@Email", email);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                customer = new BloomCustomer
                                {
                                    AccountNo = reader.GetString(reader.GetOrdinal("AccountNo")),
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    MiddleName = reader.GetString(reader.GetOrdinal("MiddleName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    Address = reader.GetString(reader.GetOrdinal("Address")),
                                    City = reader.GetString(reader.GetOrdinal("City")),
                                    Region = reader.GetString(reader.GetOrdinal("Region")),
                                    PostalCode = reader.GetString(reader.GetOrdinal("PostalCode")),
                                    Country = reader.GetString(reader.GetOrdinal("Country")),
                                    Phone = reader.GetString(reader.GetOrdinal("Phone")),
                                    BVN = reader.GetString(reader.GetOrdinal("BVN")),
                                    AccountType = reader.GetString(reader.GetOrdinal("AccountType")),
                                    IdentificationType = reader.GetString(reader.GetOrdinal("IdentificationType")),
                                    IdentificationNumber = reader.GetString(reader.GetOrdinal("IdentificationNumber")),
                                    TransactionLimit = reader.GetDecimal(reader.GetOrdinal("TransactionLimit")),
                                    DateOnboarded = reader.GetDateTime(reader.GetOrdinal("DateOnboarded")),
                                    AccountBalance = reader.GetDecimal(reader.GetOrdinal("AccountBalance")),
                                    CifNo = reader.GetString(reader.GetOrdinal("CifNo")),
                                    DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                                    HasPND = reader.GetBoolean(reader.GetOrdinal("HasPND")),
                                    CurrencyCode = reader.GetString(reader.GetOrdinal("CurrencyCode"))
                                };
                            }
                        }
                    }
                }
            }
            catch 
            {
                throw;
            }

            return customer;
        }

        public async Task<BloomCustomer> GetCustomerByPhoneNumber(string phoneNumber)
        {
            BloomCustomer customer = null;

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = @"
                    SELECT ""AccountNo"", ""FirstName"", ""MiddleName"", ""LastName"", ""Email"", ""Address"", ""City"", ""Region"", ""PostalCode"", ""Country"", ""Phone"", ""BVN"", ""AccountType"", ""IdentificationType"", ""IdentificationNumber"", ""TransactionLimit"", ""DateOnboarded"", ""AccountBalance"", ""CifNo"", ""DateOfBirth"", ""HasPND"", ""CurrencyCode""
                    FROM public.""BloomCustomers""
                    WHERE ""Phone"" = @PhoneNumber";

                        command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                customer = new BloomCustomer
                                {
                                    AccountNo = reader.GetString(reader.GetOrdinal("AccountNo")),
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    MiddleName = reader.GetString(reader.GetOrdinal("MiddleName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    Address = reader.GetString(reader.GetOrdinal("Address")),
                                    City = reader.GetString(reader.GetOrdinal("City")),
                                    Region = reader.GetString(reader.GetOrdinal("Region")),
                                    PostalCode = reader.GetString(reader.GetOrdinal("PostalCode")),
                                    Country = reader.GetString(reader.GetOrdinal("Country")),
                                    Phone = reader.GetString(reader.GetOrdinal("Phone")),
                                    BVN = reader.GetString(reader.GetOrdinal("BVN")),
                                    AccountType = reader.GetString(reader.GetOrdinal("AccountType")),
                                    IdentificationType = reader.GetString(reader.GetOrdinal("IdentificationType")),
                                    IdentificationNumber = reader.GetString(reader.GetOrdinal("IdentificationNumber")),
                                    TransactionLimit = reader.GetDecimal(reader.GetOrdinal("TransactionLimit")),
                                    DateOnboarded = reader.GetDateTime(reader.GetOrdinal("DateOnboarded")),
                                    AccountBalance = reader.GetDecimal(reader.GetOrdinal("AccountBalance")),
                                    CifNo = reader.GetString(reader.GetOrdinal("CifNo")),
                                    DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                                    HasPND = reader.GetBoolean(reader.GetOrdinal("HasPND")),
                                    CurrencyCode = reader.GetString(reader.GetOrdinal("CurrencyCode"))
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine("Error fetching customer by phone number: " + ex.Message);
                throw;
            }

            return customer;
        }

        public async Task<decimal> GetTotalSuccessfulTransactionAmountForDay(string accountNumber, DateTime date)
        {
            decimal totalAmount = 0;

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string commandText = @"
                SELECT SUM(""Amount"") AS total_amount
                FROM ""BankTransfer""
                WHERE ""SenderAccountNumber"" = @AccountNumber
                AND DATE(""TransferDate"") = @Date
                AND ""IsSuccessful"" = true
                AND ""StatusFlag"" = '1'";

                    using (var command = new NpgsqlCommand(commandText, connection))
                    {
                        command.Parameters.AddWithValue("@AccountNumber", accountNumber);
                        command.Parameters.AddWithValue("@Date", date.Date);

                        object result = await command.ExecuteScalarAsync();
                        if (result != DBNull.Value && result != null)
                        {
                            totalAmount = Convert.ToDecimal(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }

            return totalAmount;
        }

        public async Task<List<BloomCustomer>> GetCustomers()
        {
            List<BloomCustomer> customers = new List<BloomCustomer>();

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    DateTime currentDateUtc = DateTime.UtcNow.Date;

                    string sql = $@"
                SELECT ""AccountNo"", ""FirstName"", ""MiddleName"", ""LastName"", ""Email"", ""Address"", 
                       ""City"", ""Region"", ""PostalCode"", ""Country"", ""Phone"", ""BVN"", ""AccountType"", 
                       ""IdentificationType"", ""IdentificationNumber"", ""TransactionLimit"", ""DateOnboarded"", 
                       ""AccountBalance"", ""CifNo"", ""DateOfBirth"", ""HasPND"", ""CurrencyCode"", ""AccountName""
                FROM public.""BloomCustomers""
                WHERE DATE(""DateOnboarded"") = @CurrentDate";

                    using (var command = new NpgsqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@CurrentDate", NpgsqlDbType.Date, currentDateUtc);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                BloomCustomer customer = new BloomCustomer
                                {
                                    AccountNo = reader.GetString(reader.GetOrdinal("AccountNo")),
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    MiddleName = reader.GetString(reader.GetOrdinal("MiddleName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    Address = reader.GetString(reader.GetOrdinal("Address")),
                                    City = reader.GetString(reader.GetOrdinal("City")),
                                    Region = reader.GetString(reader.GetOrdinal("Region")),
                                    PostalCode = reader.GetString(reader.GetOrdinal("PostalCode")),
                                    Country = reader.GetString(reader.GetOrdinal("Country")),
                                    Phone = reader.GetString(reader.GetOrdinal("Phone")),
                                    BVN = reader.GetString(reader.GetOrdinal("BVN")),
                                    AccountType = reader.GetString(reader.GetOrdinal("AccountType")),
                                    IdentificationType = reader.GetString(reader.GetOrdinal("IdentificationType")),
                                    IdentificationNumber = reader.GetString(reader.GetOrdinal("IdentificationNumber")),
                                    TransactionLimit = reader.GetDecimal(reader.GetOrdinal("TransactionLimit")),
                                    DateOnboarded = reader.GetDateTime(reader.GetOrdinal("DateOnboarded")),
                                    AccountBalance = reader.GetDecimal(reader.GetOrdinal("AccountBalance")),
                                    CifNo = reader.GetString(reader.GetOrdinal("CifNo")),
                                    DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                                    HasPND = reader.GetBoolean(reader.GetOrdinal("HasPND")),
                                    CurrencyCode = reader.GetString(reader.GetOrdinal("CurrencyCode")),
                                    AccountName = reader.GetString(reader.GetOrdinal("AccountName"))
                                };

                                customers.Add(customer);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving customers: {ex.Message}");
                throw;
            }

            return customers;
        }

    }
}
