using BussinessLogicLayer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository
{
    public interface IPaymentRepository
    {
        Task<List<Payment>> GetAllAsync();
        Task<Payment?> GetByIdAsync(string id);
        Task AddAsync(Payment payment);
        Task UpdateAsync(Payment payment);
        Task DeleteAsync(string id);
        Task<List<Payment>> GetPendingPaymentsAsync();
    }
}
