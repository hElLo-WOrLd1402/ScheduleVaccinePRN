using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BussinessLogicLayer;
using Service;

namespace ScheduleVaccineRazor.Pages.Staff
{
    public class EditScheduleModel : PageModel
    {
        private readonly IScheduleService _scheduleService;
        private readonly IChildrenProfileService _childrenProfileService;
        private readonly IVaccineService _vaccineService;
        private readonly IAccountService _accountService;

        public EditScheduleModel(IScheduleService scheduleService, IVaccineService vaccineService, IChildrenProfileService childrenProfileService, IAccountService accountService)
        {
            _scheduleService = scheduleService;
            _childrenProfileService = childrenProfileService;
            _vaccineService = vaccineService;
            _accountService = accountService;
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
            Schedule = schedule;

            var childProfile = schedule.Child;
            var parentAccount = await _accountService.GetByIdAsync(childProfile.ParentId);

            ViewData["ChildId"] = new SelectList(parentAccount.ChildrenProfiles, "Id", "FullName");
            ViewData["VaccineId"] = new SelectList(await _vaccineService.GetAllAsync(), "Id", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _scheduleService.UpdateScheduleAsync(Schedule);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ScheduleExists(Schedule.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("/Staff/Index");
        }

        private bool ScheduleExists(string id)
        {
            if (_scheduleService.GetScheduleByIdAsync(id) != null)
            {
                return true;
            }
            return false;
        }
    }
}
