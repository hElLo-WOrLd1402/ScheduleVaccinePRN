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
        Task<Account> GetAccountByEmailAsync(string email);
        Task<bool> UpdateAccountAsync(Account account);
    }
}
