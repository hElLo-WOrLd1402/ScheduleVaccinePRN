using BussinessLogicLayer;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(Payment payment, HttpContext context);
        Task<Payment> ProcessPaymentResponseAsync(IQueryCollection query);
    }

}
