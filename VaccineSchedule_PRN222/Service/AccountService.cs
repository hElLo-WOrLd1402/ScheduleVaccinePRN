using BussinessLogicLayer;
using Repository;
using System.Threading.Tasks;

namespace Service
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository accountRepository;

        public AccountService()
        {
            accountRepository = new AccountRepository();
        }

        public async Task<Account> GetAccountByEmailAsync(string email) =>
            await accountRepository.GetAccountByEmailAsync(email);

        public async Task<bool> UpdateAccountAsync(Account account) =>
            await accountRepository.UpdateAccountAsync(account);

        // Thay đổi mật khẩu
        public async Task<bool> ChangePasswordAsync(string email, string newPassword)
        {
            var account = await accountRepository.GetAccountByEmailAsync(email);
            if (account == null)
            {
                return false;
            }

            // Hash mật khẩu trước khi lưu vào database
            account.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            return await accountRepository.UpdateAccountAsync(account);
        }

        // Thay đổi username
        public async Task<bool> ChangeUsernameAsync(string email, string newUsername)
        {
            var account = await accountRepository.GetAccountByEmailAsync(email);
            if (account == null)
            {
                return false;
            }

            account.Username = newUsername;
            return await accountRepository.UpdateAccountAsync(account);
        }
    }
}
