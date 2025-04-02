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
        private readonly IVnPayService _vnPayService;

        public SchedulePaymentModel(IScheduleService scheduleService, IVaccineService vaccineService, IPaymentService paymentService, IVnPayService vnPayService)
        {
            _scheduleService = scheduleService;
            _vaccineService = vaccineService;
            _paymentService = paymentService;
            _vnPayService = vnPayService;
        }

        [BindProperty]
        public List<Schedule> Schedules { get; set; }
        public decimal TotalAmount { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            // Lấy danh sách tất cả các lịch hẹn
            var schedules = await _scheduleService.GetAllSchedulesAsync();

            // Lọc ra các lịch hẹn có payment status là "Pending"
            var filteredSchedules = new List<Schedule>();

            foreach (var schedule in schedules)
            {
                // Lấy payment liên quan đến lịch hẹn này
                var payment = await _paymentService.GetPendingPaymentsAsync();

                // Kiểm tra nếu có payment và payment status là "Pending"
                if (payment != null && payment.Any(p => p.ScheduleId == schedule.Id && p.PaymentStatus == "Pending"))
                {
                    filteredSchedules.Add(schedule);
                }
            }

            // Cập nhật danh sách schedules cho view
            Schedules = filteredSchedules;

            if (Schedules == null || !Schedules.Any())
            {
                TempData["ErrorMessage"] = "Không có lịch hẹn nào có trạng thái thanh toán 'Chưa thanh toán'.";
            }

            // Tính tổng tiền cho các lịch hẹn đã chọn
            TotalAmount = Schedules
                          .Where(s => s.Vaccine != null)  // Tránh NullReferenceException
                          .Sum(s => s.Vaccine.Price);

            return Page();
        }
        //public async Task<IActionResult> OnPostAsync(List<string> selectedSchedules)
        //{
        //    if (selectedSchedules == null || !selectedSchedules.Any())
        //    {
        //        TempData["ErrorMessage"] = "Vui lòng chọn ít nhất một lịch hẹn để thanh toán.";
        //        return Page();
        //    }

        //    // Lấy danh sách thanh toán có trạng thái "Pending" và liên quan đến các lịch hẹn đã chọn
        //    var pendingPayments = await _paymentService.GetPendingPaymentsAsync();
        //    var selectedPayments = pendingPayments
        //        .Where(payment => selectedSchedules.Contains(payment.ScheduleId))
        //        .ToList();

        //    if (!selectedPayments.Any())
        //    {
        //        TempData["ErrorMessage"] = "Không có thanh toán nào được chọn hoặc không có thanh toán nào có trạng thái 'Pending'.";
        //        return Page();
        //    }

        //    decimal totalAmount = selectedPayments.Sum(payment => payment.Amount);

        //    // Tạo URL thanh toán sử dụng chính model Payment thay vì PaymentInformationModel
        //    string paymentUrl = _vnPayService.CreatePaymentUrl(selectedPayments.First(), HttpContext);

        //    return Redirect(paymentUrl);
        //}



        public async Task<IActionResult> OnPostAsync(List<string> selectedSchedules)
        {
            if (selectedSchedules == null || !selectedSchedules.Any())
            {
                TempData["ErrorMessage"] = "Vui lòng chọn ít nhất một lịch hẹn để thanh toán.";
                return Page();
            }

            // Lấy tất cả các thanh toán có trạng thái "Pending"
            var pendingPayments = await _paymentService.GetPendingPaymentsAsync();

            // Lọc các thanh toán có lịch hẹn đã chọn
            var selectedPayments = pendingPayments.Where(payment => selectedSchedules.Contains(payment.Schedule.Id)).ToList();

            if (!selectedPayments.Any())
            {
                TempData["ErrorMessage"] = "Không có thanh toán nào được chọn hoặc không có thanh toán nào có trạng thái 'Pending'.";
                return Page();
            }

            // Cập nhật trạng thái thanh toán của các thanh toán đã chọn
            foreach (var payment in selectedPayments)
            {
                payment.PaymentStatus = "Paid"; // Cập nhật trạng thái thanh toán thành "Paid"
                await _paymentService.UpdatePaymentAsync(payment); // Lưu thay đổi
            }

            TempData["SuccessMessage"] = "Thanh toán thành công!";
            return RedirectToPage("ScheduleIndex");
        }

    }
}