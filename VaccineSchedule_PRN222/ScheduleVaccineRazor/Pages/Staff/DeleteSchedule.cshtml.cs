using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BussinessLogicLayer;
using Service;

namespace ScheduleVaccineRazor.Pages.Staff
{
    public class DeleteScheduleModel : PageModel
    {
        private readonly IScheduleService _scheduleService;

        public DeleteScheduleModel(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [BindProperty]
        public Schedule Schedule { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _scheduleService.GetScheduleByIdAsync(id);

            if (schedule == null)
            {
                return NotFound();
            }
            else
            {
                Schedule = schedule;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _scheduleService.GetScheduleByIdAsync(id);
            if (schedule != null)
            {
                Schedule = schedule;
                await _scheduleService.DeleteScheduleAsync(schedule.Id);
            }

            return RedirectToPage("./Index");
        }
    }
}
