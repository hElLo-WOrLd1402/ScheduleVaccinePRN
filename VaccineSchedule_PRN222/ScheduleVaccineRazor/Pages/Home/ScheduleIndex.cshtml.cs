using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BussinessLogicLayer;
using Service;

namespace ScheduleVaccineRazor.Pages.Home
{
    public class ScheduleIndexModel : PageModel
    {
        private readonly IScheduleService _scheduleService;
        private readonly IChildrenProfileService _childrenProfileService;
        private readonly IVaccineService _vaccineService;

        public ScheduleIndexModel(IScheduleService scheduleService, IVaccineService vaccineService, IChildrenProfileService childrenProfileService)
        {
            _scheduleService = scheduleService;
            _childrenProfileService = childrenProfileService;
            _vaccineService = vaccineService;
        }

        public List<Schedule> Schedule { get;set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            string parentId = HttpContext.Session.GetString("ParentId");
            List<ChildrenProfile> ChildrenProfiles = await _childrenProfileService.GetAllChildrenProfilesAsync();
            ChildrenProfiles = ChildrenProfiles.FindAll(cp => cp.ParentId == parentId);

            if (ChildrenProfiles == null || !ChildrenProfiles.Any())
            {
                TempData["ErrorMessage"] = "No children profiles found.";
                return RedirectToPage("/Home/Menu");
            }

            Schedule = await _scheduleService.GetAllSchedulesAsync();
            Schedule = Schedule.Where(Schedule => Schedule.Child.ParentId == parentId).ToList();

            return Page();
        }
    }
}
