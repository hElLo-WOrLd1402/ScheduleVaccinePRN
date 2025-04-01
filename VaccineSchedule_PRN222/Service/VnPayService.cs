using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class VnPayService : IVnPayService
    {
        public string CreatePaymentUrl(PaymentInformationModel model, HttpContext context)
        {
            var vnPay = new VnPayLibrary();
            vnPay.AddRequestData("vnp_Version", "2.1.0");
            vnPay.AddRequestData("vnp_Command", "pay");
            vnPay.AddRequestData("vnp_TmnCode", "YOUR_TMN_CODE"); // Thay bằng mã TMN của bạn
            vnPay.AddRequestData("vnp_Amount", (model.Amount * 100).ToString());
            vnPay.AddRequestData("vnp_CurrCode", "VND");
            vnPay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString());
            vnPay.AddRequestData("vnp_OrderInfo", model.OrderDescription);
            vnPay.AddRequestData("vnp_OrderType", model.OrderType);
            vnPay.AddRequestData("vnp_Locale", "vn");
            vnPay.AddRequestData("vnp_ReturnUrl", "https://yourwebsite.com/VnPay/PaymentCallback");
            vnPay.AddRequestData("vnp_IpAddr", context.Connection.RemoteIpAddress?.ToString());

            string paymentUrl = vnPay.CreateRequestUrl("https://sandbox.vnpayment.vn/paymentv2/vpcpay.html", "YOUR_HASH_SECRET");
            return paymentUrl;
        }

        public PaymentResponseModel PaymentExecute(IQueryCollection collections)
        {
            var vnPay = new VnPayLibrary();
            return vnPay.GetFullResponseData(collections, "YOUR_HASH_SECRET");
        }
    }
}