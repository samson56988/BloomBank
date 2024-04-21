using DataBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CustomerDomains.Services.Interfaces
{
    public interface ICustomerRepository
    {
        Task<bool> UpdateCustomer(Guid Id, Customer customer);
        Task<bool> DeactivateCustomer(Guid Id);
        Task<bool> ActivateCustomer(string accountNo);
        Task<bool> LockCustomer(Guid Id);
        Task<bool> UnlockCustomer(Guid Id);
        Task<bool> CreateUserLoginDetails(UserLogin login);
        Task<(string message, Customer customer)> UserLogin(UserLogin login);
    }
}
