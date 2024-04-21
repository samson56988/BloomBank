using DataBase.Models;
using Domain.CustomerDomains.Services.Interfaces;
using Domain.RepositoryDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CustomerDomains.Services.Implementations
{
    public class CustomerRepository:ICustomerRepository
    {
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<UserLogin> _userLoginRepository;

        public CustomerRepository(IRepository<Customer> customerRepository, IRepository<UserLogin> userLoginRepository)
        {
            _customerRepository = customerRepository;
            _userLoginRepository = userLoginRepository;
        }

        public async Task<bool> ActivateCustomer(string accountNo)
        {
            try
            {
                var customer = await _customerRepository.GetFirstOrDefaultAsync(x => x.AccountNumber == accountNo);
                if (customer == null)
                {
                    return false;
                }
                else
                {
                    customer.IsActive = true;
                    customer.IsActivated = true;
                    await _customerRepository.UpdateAsync(customer);
                    return true;
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> CreateUserLoginDetails(UserLogin login)
        {
            try
            {
                await _userLoginRepository.AddAsync(login);
 
                return true;

            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> DeactivateCustomer(Guid Id)
        {
            try
            {
                var customer = await _customerRepository.GetFirstOrDefaultAsync(x => x.Id == Id);
                if (customer == null)
                {
                    return false;
                }
                else
                {
                    customer.IsActive = false;
                    await _customerRepository.UpdateAsync(customer);
                    return true;
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> LockCustomer(Guid Id)
        {
            try
            {
                bool IsSucccess = false;
                var customer = await _customerRepository.GetFirstOrDefaultAsync(x => x.Id == Id);
                if (customer == null)
                {
                    return IsSucccess;
                }
                else
                {
                    customer.IsLocked = false;
                    await _customerRepository.UpdateAsync(customer);
                    IsSucccess = true;
                    return IsSucccess;
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> UnlockCustomer(Guid Id)
        {
            try
            {
                var customer = await _customerRepository.GetFirstOrDefaultAsync(x => x.Id == Id);
                if (customer == null)
                {
                    return false;
                }
                else
                {
                    customer.IsLocked = false;
                    await _customerRepository.UpdateAsync(customer);
                    return true;
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> UpdateCustomer(Guid Id, Customer customer)
        {
            try
            {
                var user = await _customerRepository.GetFirstOrDefaultAsync(x => x.Id == Id);
                if (user == null)
                {
                    return false;
                }
                else
                {
                    await _customerRepository.UpdateAsync(user);
                    return true;
                }
            }
            catch
            {
                throw;
            }
  
        }

        public async Task<(string message, Customer customer)> UserLogin(UserLogin login)
        {
            try
            {
                var user = await _customerRepository.GetFirstOrDefaultAsync(x => x.AccountNumber == login.AccountNo);

                if (user == null)
                {
                    return ("Account does not exist", null);
                }
                else
                {
                    var userLoginDetails = await _userLoginRepository.GetFirstOrDefaultAsync(x => x.AccountNo == login.AccountNo);

                    if (userLoginDetails.Password.ToLower() != login.Password.ToLower())
                    {
                        return ("Incorrect password", null);
                    }
                    else
                    {
                        return ("Login Successful", user);
                    }
                }
            }
            catch
            {
                return ("Something went wrong.", null);
            }
        }
    }
}
