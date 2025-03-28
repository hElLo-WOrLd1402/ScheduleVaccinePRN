using BussinessLogicLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.DAO
{
    public class AccountDAO
    {
        public static async Task<Account> GetAccountByEmailAsync(string email)
        {
            using var db = new VaccineScheduleDbContext();
            return await db.Accounts.FirstOrDefaultAsync(c => c.Email == email);
        }

        // Create (Thêm tài khoản mới)
        public static async Task<Account> CreateAccountAsync(Account account)
        {
            using var db = new VaccineScheduleDbContext();
            db.Accounts.Add(account);
            await db.SaveChangesAsync();
            return account;
        }

        // Read (Lấy danh sách tất cả tài khoản)
        public static async Task<List<Account>> GetAllAccountsAsync()
        {
            using var db = new VaccineScheduleDbContext();
            return await db.Accounts.ToListAsync();
        }

        // Update (Cập nhật tài khoản)
        public static async Task<bool> UpdateAccountAsync(Account account)
        {
            using var db = new VaccineScheduleDbContext();
            var existingAccount = await db.Accounts.FindAsync(account.Email);
            if (existingAccount == null)
            {
                return false;
            }

            existingAccount.Username = account.Username;
            existingAccount.PasswordHash = account.PasswordHash;
            existingAccount.Role = account.Role;
            existingAccount.Status = account.Status;

            await db.SaveChangesAsync();
            return true;
        }

        // Delete (Xóa tài khoản)
        public static async Task<bool> DeleteAccountAsync(string email)
        {
            using var db = new VaccineScheduleDbContext();
            var account = await db.Accounts.FirstOrDefaultAsync(a => a.Email == email);
            if (account == null)
            {
                return false;
            }

            db.Accounts.Remove(account);
            await db.SaveChangesAsync();
            return true;
        }
    }
}
