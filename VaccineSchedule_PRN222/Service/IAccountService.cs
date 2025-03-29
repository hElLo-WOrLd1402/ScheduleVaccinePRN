using BussinessLogicLayer;
using System.Threading.Tasks;

namespace Service
{
    public interface IAccountService
    {
        Task<Account> GetAccountByEmailAsync(string email);
        Task<bool> UpdateAccountAsync(Account account);
        Task<bool> ChangePasswordAsync(string email, string newPassword);
        Task<bool> ChangeUsernameAsync(string email, string newUsername);
    }
}
