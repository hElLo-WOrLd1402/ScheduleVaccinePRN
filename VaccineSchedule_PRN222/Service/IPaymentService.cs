using BussinessLogicLayer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service
{
    public interface IPaymentService
    {
        Task<List<Payment>> GetAllPaymentsAsync();
        Task<Payment?> GetPaymentByIdAsync(string id);
        Task<bool> AddPaymentAsync(Payment payment);
        Task<bool> UpdatePaymentAsync(Payment payment);
        Task<bool> DeletePaymentAsync(string id);
    }
}
