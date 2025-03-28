using BussinessLogicLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Service;
using System.Threading.Tasks;

namespace ScheduleVaccineRazor.Pages.Home
{
    public class UpdateChildrenProfileModel : PageModel
    {
        private readonly IChildrenProfileService _childrenProfileService;

        public UpdateChildrenProfileModel()
        {
            _childrenProfileService = new ChildrenProfileService();
        }

        [BindProperty]
        public ChildrenProfile Profile { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToPage("/Home/ChildrenProfile");
            }

            Profile = await _childrenProfileService.GetChildrenProfileByIdAsync(id);
            if (Profile == null)
            {
                return RedirectToPage("/Home/ChildrenProfile");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            await _childrenProfileService.UpdateChildrenProfileAsync(Profile);
            return RedirectToPage("/Home/ChildrenProfile");
        }
    }
}
