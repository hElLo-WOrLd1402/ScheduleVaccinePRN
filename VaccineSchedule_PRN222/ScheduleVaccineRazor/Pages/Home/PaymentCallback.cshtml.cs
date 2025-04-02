using Azure.Core;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace ScheduleVaccineRazor.Pages.Home
{
    public class PaymentCallbackModel : PageModel
    {
        private readonly IPaymentService _paymentService;
        private readonly IVnPayService _vnPayService;

        public PaymentCallbackModel(IPaymentService paymentService, IVnPayService vnPayService)
        {
            _paymentService = paymentService;
            _vnPayService = vnPayService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            // Await the ProcessPaymentResponseAsync to get the actual payment response
            var paymentResponse = await _vnPayService.ProcessPaymentResponseAsync(Request.Query);

            if (paymentResponse.PaymentStatus == "Paid")
            {
                var payment = await _paymentService.GetPaymentByIdAsync(paymentResponse.Id);
                if (payment != null)
                {
                    payment.PaymentStatus = "Paid";
                    payment.PaymentDate = DateTime.Now;
                    await _paymentService.UpdatePaymentAsync(payment);
                }

                TempData["SuccessMessage"] = "Thanh toán thành công!";
            }
            else
            {
                TempData["ErrorMessage"] = "Thanh toán thất bại!";
            }

            return RedirectToPage("ScheduleIndex");
        }
    }
}
