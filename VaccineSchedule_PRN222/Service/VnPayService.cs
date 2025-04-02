using BussinessLogicLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Service
{
    public class VnPayService : IVnPayService
    {
        private readonly IConfiguration _configuration;
        private readonly PaymentService _paymentService;

        public VnPayService(IConfiguration configuration, PaymentService paymentService)
        {
            _configuration = configuration;
            _paymentService = paymentService;
        }

        public string CreatePaymentUrl(Payment payment, HttpContext context)
        {
            var vnp_TmnCode = _configuration["VNPay:TmnCode"];
            var vnp_HashSecret = _configuration["VNPay:HashSecret"];
            var vnp_Url = _configuration["VNPay:VnpUrl"];
            var vnp_ReturnUrl = _configuration["VNPay:ReturnUrl"];

            var timeNow = DateTime.UtcNow.AddHours(7);
            var pay = new SortedList<string, string>
            {
                { "vnp_Version", "2.1.0" },
                { "vnp_Command", "pay" },
                { "vnp_TmnCode", vnp_TmnCode },
                { "vnp_Amount", (payment.Amount * 100).ToString() }, // VNPay yêu cầu số tiền nhân 100
                { "vnp_CurrCode", "VND" },
                { "vnp_TxnRef", payment.Id },
                { "vnp_OrderInfo", $"Thanh toán lịch hẹn {payment.ScheduleId}" },
                { "vnp_OrderType", "billpayment" },
                { "vnp_Locale", "vn" },
                { "vnp_ReturnUrl", vnp_ReturnUrl },
                { "vnp_IpAddr", context.Connection.RemoteIpAddress?.ToString() },
                { "vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss") }
            };

            var query = new StringBuilder();
            foreach (var kv in pay)
            {
                query.Append(HttpUtility.UrlEncode(kv.Key) + "=" + HttpUtility.UrlEncode(kv.Value) + "&");
            }

            var rawData = query.ToString().TrimEnd('&');
            var secureHash = HmacSha512(vnp_HashSecret, rawData);
            var paymentUrl = $"{vnp_Url}?{rawData}&vnp_SecureHash={secureHash}";

            return paymentUrl;
        }

        public async Task<Payment> ProcessPaymentResponseAsync(IQueryCollection query)
        {
            var vnp_HashSecret = _configuration["VNPay:HashSecret"];
            var vnp_ResponseCode = query["vnp_ResponseCode"];
            var vnp_TxnRef = query["vnp_TxnRef"];
            var vnp_SecureHash = query["vnp_SecureHash"];

            var sortedData = new SortedList<string, string>();
            foreach (var key in query.Keys)
            {
                if (key != "vnp_SecureHash" && key != "vnp_SecureHashType")
                {
                    sortedData.Add(key, query[key]);
                }
            }

            var rawData = new StringBuilder();
            foreach (var kv in sortedData)
            {
                rawData.Append(kv.Key + "=" + kv.Value + "&");
            }

            var checkSum = HmacSha512(vnp_HashSecret, rawData.ToString().TrimEnd('&'));

            // Use PaymentService to get the payment
            var payment = await _paymentService.GetPaymentByIdAsync(vnp_TxnRef);
            if (payment == null)
            {
                throw new Exception("Không tìm thấy giao dịch thanh toán.");
            }

            if (checkSum.Equals(vnp_SecureHash, StringComparison.InvariantCultureIgnoreCase))
            {
                payment.PaymentStatus = vnp_ResponseCode == "00" ? "Paid" : "Failed";
                payment.PaymentDate = DateTime.UtcNow.AddHours(7);

                // Use PaymentService to update the payment
                await _paymentService.UpdatePaymentAsync(payment);
            }
            else
            {
                throw new Exception("Chuỗi kiểm tra bảo mật không hợp lệ!");
            }

            return payment;
        }

        private string HmacSha512(string key, string inputData)
        {
            using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key));
            return BitConverter.ToString(hmac.ComputeHash(Encoding.UTF8.GetBytes(inputData))).Replace("-", "").ToLower();
        }
    }
}
