using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BussinessLogicLayer;
using Service;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.SignalR;
using static ScheduleVaccineRazor.Pages.Home.ScheduleIndexModel;

namespace ScheduleVaccineRazor.Pages.Home
{
    public class CreateScheduleModel : PageModel
    {
        private readonly IScheduleService _scheduleService;
        private readonly IChildrenProfileService _childrenProfileService;
        private readonly IVaccineService _vaccineService;

        public CreateScheduleModel(IScheduleService scheduleService, IVaccineService vaccineService, IChildrenProfileService childrenProfileService)
        {
            _scheduleService = scheduleService;
            _childrenProfileService = childrenProfileService;
            _vaccineService = vaccineService;
        }
        public List<ChildrenProfile> ChildrenProfiles { get; set; } = new();


        public async Task<IActionResult> OnGet()
        {
            string parentId = HttpContext.Session.GetString("ParentId");
            ChildrenProfiles = await _childrenProfileService.GetAllChildrenProfilesAsync();
            ChildrenProfiles = ChildrenProfiles.FindAll(cp => cp.ParentId == parentId);

            if (ChildrenProfiles == null || !ChildrenProfiles.Any())
            {
                TempData["ErrorMessage"] = "No children profiles found.";
                return RedirectToPage("/Home/ScheduleIndex");
            }

            ViewData["ChildId"] = new SelectList(ChildrenProfiles, "Id", "FullName");
            ViewData["VaccineId"] = new SelectList(await _vaccineService.GetAllAsync(), "Id", "Name");
            return Page();
        }

        [BindProperty]
        public CreateScheduleInput Schedule { get; set; } = default!;



        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return RedirectToPage("/Home/ScheduleIndex");
            }


            var ScheduleId = await GenerateID();

            var newSchedule = new Schedule
            {
                Id = ScheduleId,
                ChildId = Schedule.ChildId,
                VaccineId = Schedule.VaccineId,
                Status = "Scheduled",
                AppointmentDate = Schedule.AppointmentDate,
            };
            // Gửi thông báo SignalR khi thuốc mới được thêm
            var hubContext = HttpContext.RequestServices.GetRequiredService<IHubContext<SignalrServer>>();
            await hubContext.Clients.All.SendAsync("ItemCreated", ScheduleId);
            await _scheduleService.AddScheduleAsync(newSchedule);

            return RedirectToPage("/Home/ScheduleIndex");
        }

        private async Task<string> GenerateID()
        {
            var schedules = await _scheduleService.GetAllSchedulesAsync();
            if (schedules == null || !schedules.Any())
            {
                return "S00001";
            }

            int maxOrder = schedules
                .Select(a => GetScheduleOrder(a.Id))
                .Max();

            return $"S{(maxOrder + 1).ToString().PadLeft(5, '0')}";
        }

        private int GetScheduleOrder(string scheduleId)
        {
            if (string.IsNullOrWhiteSpace(scheduleId) || scheduleId.Length < 2)
            {
                return 0;
            }

            string orderPart = scheduleId.Substring(1);
            return int.TryParse(orderPart, out int orderNumber) ? orderNumber : 0;
        }
    }

    public class CreateScheduleInput1
    {
        [Required]
        public string ChildId { get; set; } = string.Empty;

        [Required]
        public string VaccineId { get; set; } = string.Empty;

        [Required]
        public DateTime AppointmentDate { get; set; } = DateTime.Now.AddDays(7);
    }
}