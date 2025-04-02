using BussinessLogicLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;
using System.Security.Principal;

namespace ScheduleVaccineRazor.Pages.Accounts
{
    public class LoginModel : PageModel
    {
        private readonly IAccountService _accountService;

        [BindProperty]
        public Account Input { get; set; } = new Account();//lấy input
        public bool LoginFailed { get; set; } = false;

        public LoginModel(IAccountService storeAccountService)
        {
            _accountService = storeAccountService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Fetch user by Email
            var user = await _accountService.GetAccountByEmailAsync(Input.Email);

            if (user != null && user.PasswordHash == Input.PasswordHash)
            {
                if (user.Role == "Admin")
                {
                    // Store session information
                    HttpContext.Session.SetString("Username", user.Username);
                    HttpContext.Session.SetString("RoleId", user.Role.ToString());
                    return RedirectToPage("/Dashboard/Index");
                }
                else if (user.Role == "Staff") 
                {
                    HttpContext.Session.SetString("Username", user.Username);
                    HttpContext.Session.SetString("RoleId", user.Role.ToString());
                    return RedirectToPage("/Staff/Index");
                }
                else if (user.Role == "Customer")
                {
                    HttpContext.Session.SetString("Username", user.Username);
                    HttpContext.Session.SetString("RoleId", user.Role.ToString());
                    HttpContext.Session.SetString("ParentId", user.Id); // Giả sử user.Id là ParentId                  
                    return RedirectToPage("/Home/Menu");
                }
                else
                {
                    LoginFailed = true;
                    return Page();
                }
            }
            else
            {
                LoginFailed = true;
                return Page();
            }
        }
    }
}