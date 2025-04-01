using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BussinessLogicLayer;
using Service;
using System.ComponentModel.DataAnnotations;

namespace ScheduleVaccineRazor.Pages.Home
{
    public class ScheduleIndexModel : PageModel
    {
        private readonly IScheduleService _scheduleService;
        private readonly IChildrenProfileService _childrenProfileService;
        private readonly IVaccineService _vaccineService;
        private readonly IPaymentService _paymentService; // Thêm PaymentService vào

        public ScheduleIndexModel(
            IScheduleService scheduleService,
            IVaccineService vaccineService,
            IChildrenProfileService childrenProfileService,
            IPaymentService paymentService)  // Inject IPaymentService
        {
            _scheduleService = scheduleService;
            _childrenProfileService = childrenProfileService;
            _vaccineService = vaccineService;
            _paymentService = paymentService;  // Gán PaymentService vào
        }

        public List<Schedule> Schedule { get; set; } = new();
        public List<ChildrenProfile> ChildrenProfiles { get; set; } = new(); // Danh sách hồ sơ trẻ
        public List<Vaccine> Vaccines { get; set; } = new(); // Danh sách vaccine
        public List <Payment> Payments { get; set; } = new();
        [BindProperty]
        public CreateScheduleInput ScheduleInput { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            string parentId = HttpContext.Session.GetString("ParentId");

            // Lấy danh sách hồ sơ trẻ của phụ huynh hiện tại
            ChildrenProfiles = await _childrenProfileService.GetAllChildrenProfilesAsync();
            ChildrenProfiles = ChildrenProfiles.Where(cp => cp.ParentId == parentId).ToList();

            if (!ChildrenProfiles.Any())
            {
                TempData["ErrorMessage"] = "No children profiles found.";
                return RedirectToPage("/Home/Menu");
            }

            // Lấy danh sách lịch hẹn của trẻ thuộc phụ huynh hiện tại
            Schedule = await _scheduleService.GetAllSchedulesAsync();
            Schedule = Schedule.Where(s => s.Child.ParentId == parentId).ToList();

            // Lấy danh sách vaccine
            Vaccines = await _vaccineService.GetAllAsync();

            // Lấy thông tin thanh toán từ PaymentService
            foreach (var schedule in Schedule)
            {
                // Lấy tất cả các payment có PaymentStatus là "Pending"
                var pendingPayments = await _paymentService.GetPendingPaymentsAsync();          
            }
            return Page();
        }




        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid input.";
                return RedirectToPage();
            }

            var scheduleId = await GenerateID();

            var newSchedule = new Schedule
            {
                Id = scheduleId,
                ChildId = ScheduleInput.ChildId,
                VaccineId = ScheduleInput.VaccineId,
                Status = "Scheduled",
                AppointmentDate = ScheduleInput.AppointmentDate,
            };

            await _scheduleService.AddScheduleAsync(newSchedule);
            TempData["SuccessMessage"] = "Lịch hẹn đã được đặt thành công!";
            return RedirectToPage();
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

    public class CreateScheduleInput
    {
        [Required]
        public string ChildId { get; set; } = string.Empty;

        [Required]
        public string VaccineId { get; set; } = string.Empty;

        [Required]
        public DateTime AppointmentDate { get; set; } = DateTime.Now.AddDays(7);
    }
}
