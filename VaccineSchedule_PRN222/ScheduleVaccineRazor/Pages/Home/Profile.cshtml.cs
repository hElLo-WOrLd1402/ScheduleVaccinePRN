using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;

namespace ScheduleVaccineRazor.Pages.Home
{
    public class ProfileModel : PageModel
    {
        private readonly IAccountService _accountService;

        [BindProperty]
        public string NewUsername { get; set; }

        [BindProperty]
        public string NewPassword { get; set; }

        public ProfileModel()
        {
            _accountService = new AccountService();
        }

        public void OnGet()
        {
            // Hiển thị profile, nếu cần.
        }

        public async Task<IActionResult> OnPostChangeUsernameAsync(string email)
        {
            bool result = await _accountService.ChangeUsernameAsync(email, NewUsername);
            if (result)
            {
                TempData["Success"] = "Username updated successfully!";
            }
            else
            {
                TempData["Error"] = "Failed to update username.";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostChangePasswordAsync(string email)
        {
            bool result = await _accountService.ChangePasswordAsync(email, NewPassword);
            if (result)
            {
                TempData["Success"] = "Password updated successfully!";
            }
            else
            {
                TempData["Error"] = "Failed to update password.";
            }

            return Page();
        }
    }
}
