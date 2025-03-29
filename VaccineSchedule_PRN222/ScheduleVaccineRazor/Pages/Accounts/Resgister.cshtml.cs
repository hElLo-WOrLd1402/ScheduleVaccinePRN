using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BussinessLogicLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;

namespace ScheduleVaccineRazor.Pages.Accounts
{
    public class RegisterModel : PageModel
    {
        private readonly IAccountService _accountService;

        public RegisterModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [BindProperty]
        public RegisterInputModel RegisterData { get; set; } = new RegisterInputModel();

        public string? ErrorMessage { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                // Kiểm tra email đã tồn tại chưa
                var existingAccount = await _accountService.GetAccountByEmailAsync(RegisterData.Email);
                if (existingAccount != null)
                {
                    ModelState.AddModelError("RegisterData.Email", "Email already exists.");
                    return Page();
                }

                // Tạo ID mới theo format A00001++
                var newAccountId = await GenerateAccountIdAsync();

                // Tạo tài khoản mới
                var newAccount = new Account
                {
                    Id = newAccountId,
                    Email = RegisterData.Email,
                    Username = RegisterData.Username,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(RegisterData.Password), // Hash mật khẩu
                    Role = "Customer",
                    Status = "Active",
                    CreatedTime = DateTime.UtcNow,
                    LastUpdatedTime = DateTime.UtcNow
                };

                await _accountService.CreateAccountAsync(newAccount);
                return RedirectToPage("/Home/Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
        }

        // Tạo ID tự động theo format A00001++
        private async Task<string> GenerateAccountIdAsync()
        {
            var accounts = await _accountService.GetAllAccountsAsync();
            if (accounts == null || !accounts.Any())
            {
                return "A00001"; // Nếu chưa có tài khoản nào, bắt đầu từ A00001
            }

            int maxOrder = accounts
                .Select(a => GetAccountOrder(a.Id))
                .Max();

            return $"A{(maxOrder + 1).ToString().PadLeft(5, '0')}"; // Đảm bảo luôn có 5 chữ số
        }

        private int GetAccountOrder(string accountId)
        {
            if (string.IsNullOrWhiteSpace(accountId) || accountId.Length < 2)
            {
                return 0;
            }

            string orderPart = accountId.Substring(1); // Lấy số sau "A"
            return int.TryParse(orderPart, out int orderNumber) ? orderNumber : 0;
        }
    }

    public class RegisterInputModel
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required, DataType(DataType.Password), Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
