using BussinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IAccountRepository
    {
        Task<List<Account>> GetAllAccountsAsync();
        Task<Account> GetAccountByEmailAsync(string email);
        Task<bool> UpdateAccountAsync(Account account);
        Task<Account> CreateAccountAsync(Account account);
        Task<Account?> GetByIdAsync(string id);
    }
}
