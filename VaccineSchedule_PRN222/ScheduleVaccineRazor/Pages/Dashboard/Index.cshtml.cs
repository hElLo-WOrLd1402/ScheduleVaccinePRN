using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;
using BussinessLogicLayer;

namespace ScheduleVaccineRazor.Pages.Dashboard
{
    public class IndexModel : PageModel
    {
        private readonly IScheduleService _scheduleService;
        private readonly IPaymentService _paymentService;
    public IndexModel(IScheduleService scheduleService, IPaymentService paymentService)
        {
            _scheduleService = scheduleService;
            _paymentService = paymentService;
        }

        public int TotalAppointments { get; set; }
        public decimal TotalRevenue { get; set; }
        public List<RevenueData> RevenueByMonth { get; set; }

        public async Task OnGetAsync()
        {
            // Lấy tổng số lịch hẹn đã đặt
            var schedules = await _scheduleService.GetAllSchedulesAsync();
            TotalAppointments = schedules.Count;

            // Lấy danh sách thanh toán đã hoàn thành
            var payments = await _paymentService.GetAllPaymentsAsync();
            var completedPayments = payments.Where(p => p.PaymentStatus == "Paid").ToList();

            // Tính tổng doanh thu
            TotalRevenue = completedPayments.Sum(p => p.Amount);

            // Lấy doanh thu theo tháng
            RevenueByMonth = completedPayments
                .Where(p => p.PaymentDate.HasValue)
                .GroupBy(p => new {
                    Year = p.PaymentDate.Value.Year,
                    Month = p.PaymentDate.Value.Month
                })
                .Select(g => new RevenueData
                {
                    Month = $"{g.Key.Month}/{g.Key.Year}", // Định dạng "Tháng/Năm"
                    Year = g.Key.Year,
                    MonthNumber = g.Key.Month,
                    Revenue = g.Sum(p => p.Amount)
                })
                .OrderBy(g => g.Year)
                .ThenBy(g => g.MonthNumber)
                .ToList();

            // Đảm bảo không trả về null
            RevenueByMonth ??= new List<RevenueData>();

        }
        public class RevenueData
        {
            public string Month { get; set; }
            public int Year { get; set; }  // Để sắp xếp theo năm
            public int MonthNumber { get; set; }  // Để sắp xếp theo tháng
            public decimal Revenue { get; set; }
        }

    }

}