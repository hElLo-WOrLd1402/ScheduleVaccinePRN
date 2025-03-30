using BussinessLogicLayer;
using DataAccessLayer.DAO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly PaymentDAO _paymentDAO;

        public PaymentRepository()
        {
            _paymentDAO = new PaymentDAO();
        }

        public async Task<List<Payment>> GetAllAsync() =>
            await _paymentDAO.GetAllPaymentAsync();

        public async Task<Payment?> GetByIdAsync(string id) =>
            await _paymentDAO.GetPaymentByIdAsync(id);

        public async Task AddAsync(Payment payment) =>
            await _paymentDAO.AddAsync(payment);

        public async Task UpdateAsync(Payment payment) =>
            await _paymentDAO.UpdateAsync(payment);

        public async Task DeleteAsync(string id) =>
            await _paymentDAO.DeletePaymentAsync(id);
        public async Task<List<Payment>> GetPendingPaymentsAsync() =>
            await _paymentDAO.GetPendingPaymentsAsync();
    }
}
