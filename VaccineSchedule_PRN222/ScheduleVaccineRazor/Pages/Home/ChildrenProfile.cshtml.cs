using BussinessLogicLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http; // Để sử dụng Session
using Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScheduleVaccineRazor.Pages.Home
{
    public class ChildrenProfileModel : PageModel
    {
        private readonly IChildrenProfileService _childrenProfileService;

        public ChildrenProfileModel()
        {
            _childrenProfileService = new ChildrenProfileService();
        }

        public List<ChildrenProfile> ChildrenProfiles { get; set; } = new();

        [BindProperty]
        public ChildrenProfile NewProfile { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            // Lấy ParentId từ Session
            string parentId = HttpContext.Session.GetString("ParentId");
            if (string.IsNullOrEmpty(parentId))
            {
                return RedirectToPage("/Account/Login"); // Chuyển hướng nếu chưa đăng nhập
            }

            // Gán ParentId vào Model để khi submit form không bị lỗi
            NewProfile.ParentId = parentId;

            // Lọc danh sách hồ sơ trẻ em theo ParentId
            ChildrenProfiles = await _childrenProfileService.GetAllChildrenProfilesAsync();
            ChildrenProfiles = ChildrenProfiles.FindAll(cp => cp.ParentId == parentId);

            return Page();
        }

        public async Task<IActionResult> OnPostAddAsync()
        {
            // Lấy ParentId từ Session
            string parentId = HttpContext.Session.GetString("ParentId");
            if (string.IsNullOrEmpty(parentId))
            {
                ModelState.AddModelError("", "Không xác định được Parent ID.");
                return Page();
            }

            // Gán ParentId vào hồ sơ mới trước khi kiểm tra ModelState
            NewProfile.ParentId = parentId;

            // Xóa trạng thái lỗi nếu ParentId bị thiếu
            ModelState.ClearValidationState(nameof(NewProfile.ParentId));

            // Kiểm tra ModelState sau khi gán giá trị hợp lệ
            if (!TryValidateModel(NewProfile))
            {
                return Page();
            }

            await _childrenProfileService.AddChildrenProfileAsync(NewProfile);
            return RedirectToPage();
        }


        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            await _childrenProfileService.DeleteChildrenProfileAsync(id);
            return RedirectToPage();
        }
    }
}
