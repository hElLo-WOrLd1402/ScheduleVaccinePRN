using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Service;
using BussinessLogicLayer;
using System.Collections.Generic;
using System.Linq;

namespace ScheduleVaccineRazor.Pages.Home
{
    public class SchedulePaymentModel : PageModel
    {
        private readonly IScheduleService _scheduleService;
        private readonly IVaccineService _vaccineService;
        private readonly IPaymentService _paymentService;

        public SchedulePaymentModel(IScheduleService scheduleService, IVaccineService vaccineService, IPaymentService paymentService)
        {
            _scheduleService = scheduleService;
            _vaccineService = vaccineService;
            _paymentService = paymentService;
        }

        [BindProperty]
        public List<Schedule> Schedules { get; set; }
        public decimal TotalAmount { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var payments = await _paymentService.GetPendingPaymentsAsync();
            Schedules = await _scheduleService.GetAllSchedulesAsync(); // Sửa lỗi await

            // Kiểm tra nếu danh sách null
            if (Schedules == null)
            {
                Schedules = new List<Schedule>();
            }

            TotalAmount = Schedules
                         .Where(s => s != null && s.Vaccine != null) // Tránh NullReferenceException
                         .Sum(s => s.Vaccine.Price);

            return Page();
        }


        public async Task<IActionResult> OnPostAsync(List<string> selectedSchedules)
        {
            if (selectedSchedules == null || !selectedSchedules.Any())
            {
                TempData["ErrorMessage"] = "Vui lòng chọn ít nhất một lịch hẹn để thanh toán.";
                return Page();
            }

            foreach (var scheduleId in selectedSchedules)
            {
                // Lấy Payment liên quan đến lịch hẹn
                var payment = await _paymentService.GetPaymentByIdAsync(scheduleId);

                if (payment != null)
                {
                    payment.PaymentStatus = "Paid"; // Chỉ cập nhật status của Payment
                    await _paymentService.UpdatePaymentAsync(payment);
                }
            }

            TempData["SuccessMessage"] = "Thanh toán thành công!";
            return RedirectToPage("ScheduleIndex");
        }

    }
}