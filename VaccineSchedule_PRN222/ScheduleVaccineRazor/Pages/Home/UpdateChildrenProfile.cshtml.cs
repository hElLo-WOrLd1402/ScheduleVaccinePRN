using BussinessLogicLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;
using System.Threading.Tasks;

namespace ScheduleVaccineRazor.Pages.Home
{
    public class UpdateChildrenProfileModel : PageModel
    {
        private readonly IChildrenProfileService _childrenProfileService;

        public UpdateChildrenProfileModel(IChildrenProfileService childrenProfileService)
        {
            _childrenProfileService = childrenProfileService;
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
            if (!ModelState.IsValid) 
            {

                foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(modelError.ErrorMessage); // Ghi lỗi vào console (Kiểm tra Output trong Debug)
                }
                return Page(); }

            // Lấy dữ liệu cũ từ database
            var existingProfile = await _childrenProfileService.GetChildrenProfileByIdAsync(Profile.Id);
            if (existingProfile == null)
            {
                return NotFound();
            }

            // Chỉ cập nhật FullName và BirthDate, giữ nguyên các giá trị khác
            existingProfile.FullName = !string.IsNullOrWhiteSpace(Profile.FullName) ? Profile.FullName : existingProfile.FullName;

            // Kiểm tra ngày sinh không lớn hơn ngày hiện tại
            if (Profile.BirthDate > DateOnly.FromDateTime(DateTime.Today))
            {
                ModelState.AddModelError("Profile.BirthDate", "BirthDate cannot be in the future.");
                return Page();
            }

            // Cập nhật ngày sinh nếu hợp lệ
            if (Profile.BirthDate != default)
            {
                existingProfile.BirthDate = Profile.BirthDate;
            }

            // Giữ nguyên ParentId, Gender, và Schedules
            existingProfile.ParentId = Profile.ParentId;
            existingProfile.Gender = existingProfile.Gender;
            existingProfile.Schedules = existingProfile.Schedules;

            await _childrenProfileService.UpdateChildrenProfileAsync(existingProfile);
            return RedirectToPage("/Home/ChildrenProfile");
        }
    }
}
