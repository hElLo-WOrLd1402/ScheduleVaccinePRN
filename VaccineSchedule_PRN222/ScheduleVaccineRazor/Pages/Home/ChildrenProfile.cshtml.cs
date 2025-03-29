using BussinessLogicLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http; // Để sử dụng Session
using Service;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BussinessLogicLayer;

namespace ScheduleVaccineRazor.Pages.Home
{
    public class ChildrenProfileModel : PageModel
    {
        private readonly IChildrenProfileService _childrenProfileService;

        public ChildrenProfileModel(IChildrenProfileService childrenProfileService)
        {
            _childrenProfileService = childrenProfileService;
        }

        [BindProperty]
        public ChildrenProfile NewProfile { get; set; } = new();

        public List<ChildrenProfile> ChildrenProfiles { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            string parentId = HttpContext.Session.GetString("ParentId");
            if (string.IsNullOrEmpty(parentId))
            {
                return RedirectToPage("/Account/Login");
            }

            NewProfile.ParentId = parentId;
            NewProfile.Id = await GenerateChildIDAsync(); // Tạo ID mới trước khi hiển thị form

            ChildrenProfiles = await _childrenProfileService.GetAllChildrenProfilesAsync();
            ChildrenProfiles = ChildrenProfiles.FindAll(cp => cp.ParentId == parentId);

            return Page();
        }

        public async Task<IActionResult> OnPostAddAsync()
        {
            string parentId = HttpContext.Session.GetString("ParentId");
            if (string.IsNullOrEmpty(parentId))
            {
                ModelState.AddModelError("", "Không xác định được Parent ID.");
                return Page();
            }

            NewProfile.ParentId = parentId;
            NewProfile.Id = await GenerateChildIDAsync(); // Sinh ID trước khi lưu

            await _childrenProfileService.AddChildrenProfileAsync(NewProfile);

            ChildrenProfiles = await _childrenProfileService.GetAllChildrenProfilesAsync();
            ChildrenProfiles = ChildrenProfiles.FindAll(cp => cp.ParentId == parentId);

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            await _childrenProfileService.DeleteChildrenProfileAsync(id);
            return RedirectToPage();
        }

        // 🔹 Tạo ID dạng C00001, C00002, ...
        private async Task<string> GenerateChildIDAsync()
        {
            var profiles = await _childrenProfileService.GetAllChildrenProfilesAsync();
            if (profiles == null || !profiles.Any())
            {
                return "C00001";
            }

            int maxOrder = profiles
                .Select(p => GetChildOrder(p.Id))
                .Max();

            return $"C{(maxOrder + 1).ToString().PadLeft(5, '0')}";
        }

        private int GetChildOrder(string childId)
        {
            if (string.IsNullOrWhiteSpace(childId) || childId.Length < 2)
            {
                return 0;
            }

            string orderPart = childId.Substring(1); // Lấy phần số sau 'C'
            return int.TryParse(orderPart, out int orderNumber) ? orderNumber : 0;
        }
    }
}
