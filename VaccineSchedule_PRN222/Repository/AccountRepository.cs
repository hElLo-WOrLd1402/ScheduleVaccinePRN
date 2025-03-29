using BussinessLogicLayer;
using DataAccessLayer.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class AccountRepository : IAccountRepository
    {
        public async Task<Account> CreateAccountAsync(Account account) =>
          await AccountDAO.CreateAccountAsync(account);

        public async Task<Account> GetAccountByEmailAsync(string email) =>
          await AccountDAO.GetAccountByEmailAsync(email);

        public async Task<List<Account>> GetAllAccountsAsync() =>
          await AccountDAO.GetAllAccountsAsync();


        public async Task<bool> UpdateAccountAsync(Account account) =>
          await AccountDAO.UpdateAccountAsync(account);
    }
}