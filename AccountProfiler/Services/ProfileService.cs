using AccountProfiler.Models;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountProfiler.Services
{
    public class ProfileService
    {
        public async Task<List<BloomBankCustomer>> GetCustomers()
        {
            List<BloomBankCustomer> customers = new List<BloomBankCustomer>();

            try
            {
                using (var connection = new NpgsqlConnection(ConfigurationManager.AppSettings["DatabaseConnection"]))
                {
                    await connection.OpenAsync();

                    DateTime currentDateUtc = DateTime.UtcNow.Date;

                    string sql = $@"
                SELECT ""AccountNo"", ""FirstName"", ""MiddleName"", ""LastName"", ""Email"", ""Address"", 
                       ""City"", ""Region"", ""PostalCode"", ""Country"", ""Phone"", ""BVN"", ""AccountType"", 
                       ""IdentificationType"", ""IdentificationNumber"", ""TransactionLimit"", ""DateOnboarded"", 
                       ""AccountBalance"", ""CifNo"", ""DateOfBirth"", ""HasPND"", ""CurrencyCode"", ""AccountName""
                FROM public.""BloomCustomers""";
                

                    using (var command = new NpgsqlCommand(sql, connection))
                    {
                        //command.Parameters.AddWithValue("@CurrentDate", NpgsqlDbType.Date, currentDateUtc);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                BloomBankCustomer customer = new BloomBankCustomer
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

        public async Task<bool> IsAccountExistsOnBloomPortal(string accountNo)
        {
            string connectionString = ConfigurationManager.AppSettings["BloomPortalDb"];

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string sql = @"
                SELECT COUNT(*)
                FROM public.""Customers""
                WHERE ""AccountNumber"" = @AccountNo";

                    using (var command = new NpgsqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@AccountNo", accountNo);

                        object result = await command.ExecuteScalarAsync();
                        long count = Convert.ToInt64(result); // Convert to Int64 (long)

                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while checking account existence: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> IsAccountExistsOnUssd(string accountNo)
        {
            string connectionString = ConfigurationManager.AppSettings["BloomUSSDDb"];

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string sql = @"
                SELECT COUNT(*)
                FROM public.""Customers""
                WHERE ""AccountNumber"" = @AccountNo";

                    using (var command = new NpgsqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@AccountNo", accountNo);

                        object result = await command.ExecuteScalarAsync();
                        long count = Convert.ToInt64(result); // Convert to Int64 (long)

                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while checking account existence: {ex.Message}");
                throw;
            }
        }

        public async Task<BloomBankCustomer> ProfileCustomerOnWebPlatform(BloomBankCustomer customer)
        {
            try
            {
                string connectionString = ConfigurationManager.AppSettings["BloomPortalDb"];

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = @"
                INSERT INTO public.""Customers"" (""Id"",""AccountNumber"", ""FirstName"", ""MiddleName"", ""LastName"", ""Email"", ""Address"", ""City"", ""Region"", ""PostalCode"", ""Country"", ""Phone"", ""BVN"", ""AccountType"", ""IdentificationType"", ""IdenitificationNumber"", ""TransactionLimit"", ""DateOnboarded"", ""AccountBalance"", ""DateOfBirth"", ""AccountName"")
                    VALUES (@Id,@AccountNo, @FirstName, @MiddleName, @LastName, @Email, @Address, @City, @Region, @PostalCode, @Country, @Phone, @BVN, @AccountType, @IdentificationType, @IdentificationNumber, @TransactionLimit, @DateOnboarded, @AccountBalance, @DateOfBirth, @AccountName);
                    ";
                        command.Parameters.AddWithValue("@Id", Guid.NewGuid());
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
                        command.Parameters.AddWithValue("@DateOfBirth", customer.DateOfBirth);
                        command.Parameters.AddWithValue("@AccountName", customer.AccountName);

                        int rowsAffected = await command.ExecuteNonQueryAsync();

                        return customer;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<BloomBankCustomer> ProfileCustomerOnUssdPlatform(BloomBankCustomer customer)
        {
            try
            {
                string connectionString = ConfigurationManager.AppSettings["BloomUSSDDb"];

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = @"
                INSERT INTO public.""Customers"" (""Id"",""AccountNumber"", ""FirstName"", ""MiddleName"", ""LastName"", ""Email"", ""Address"", ""City"", ""Region"", ""PostalCode"", ""Country"", ""Phone"", ""BVN"", ""AccountType"", ""IdentificationType"", ""IdenitificationNumber"", ""TransactionLimit"", ""DateOnboarded"", ""AccountBalance"", ""DateOfBirth"", ""AccountName"")
                    VALUES (@Id,@AccountNo, @FirstName, @MiddleName, @LastName, @Email, @Address, @City, @Region, @PostalCode, @Country, @Phone, @BVN, @AccountType, @IdentificationType, @IdentificationNumber, @TransactionLimit, @DateOnboarded, @AccountBalance, @DateOfBirth, @AccountName);
                    ";
                        command.Parameters.AddWithValue("@Id", Guid.NewGuid());
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
                        command.Parameters.AddWithValue("@DateOfBirth", customer.DateOfBirth);
                        command.Parameters.AddWithValue("@AccountName", customer.AccountName);

                        int rowsAffected = await command.ExecuteNonQueryAsync();

                        return customer;
                    }
                }
            }
            catch
            {
                throw;
            }
        }


    }
}
