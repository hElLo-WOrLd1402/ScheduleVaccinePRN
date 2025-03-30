using BussinessLogicLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;

namespace ScheduleVaccineRazor.Pages.Staff
{
    public class IndexModel : PageModel
    {
        private readonly IScheduleService _scheduleService;
        private readonly IChildrenProfileService _childrenProfileService;
        private readonly IVaccineService _vaccineService;

        public IndexModel(IScheduleService scheduleService, IVaccineService vaccineService, IChildrenProfileService childrenProfileService)
        {
            _scheduleService = scheduleService;
            _childrenProfileService = childrenProfileService;
            _vaccineService = vaccineService;
        }

        public List<Schedule> Schedule { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            Schedule = await _scheduleService.GetAllSchedulesAsync();

            return Page();
        }
    }
}
