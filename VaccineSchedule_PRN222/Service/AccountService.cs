using BussinessLogicLayer;
using DataAccessLayer.DAO;
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
        public async Task<Account?> GetByIdAsync(string id)
        {
            return await accountRepository.GetByIdAsync(id);

        }
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

        // ✅ Thêm CreateAccountAsync()
        public async Task<Account> CreateAccountAsync(Account account)
        {
            // Kiểm tra email đã tồn tại chưa
            var existingAccount = await accountRepository.GetAccountByEmailAsync(account.Email);
            if (existingAccount != null)
            {
                throw new Exception("Email already exists.");
            }

            // Hash mật khẩu trước khi lưu (nếu có)
            account.PasswordHash = BCrypt.Net.BCrypt.HashPassword(account.PasswordHash);

            return await accountRepository.CreateAccountAsync(account);
        }

        public async Task<List<Account>> GetAllAccountsAsync() =>
            await accountRepository.GetAllAccountsAsync();
    }
}
