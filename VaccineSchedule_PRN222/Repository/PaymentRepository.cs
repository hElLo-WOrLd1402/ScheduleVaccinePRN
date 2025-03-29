using BussinessLogicLayer;
using DataAccessLayer.DAO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly PaymentDAO _paymentDAO;

        public PaymentRepository(VaccineScheduleDbContext context)
        {
            _paymentDAO = new PaymentDAO(context);
        }

        public async Task<List<Payment>> GetAllAsync() =>
            await _paymentDAO.GetAllAsync();

        public async Task<Payment?> GetByIdAsync(string id) =>
            await _paymentDAO.GetByIdAsync(id);

        public async Task AddAsync(Payment payment) =>
            await _paymentDAO.AddAsync(payment);

        public async Task UpdateAsync(Payment payment) =>
            await _paymentDAO.UpdateAsync(payment);

        public async Task DeleteAsync(string id) =>
            await _paymentDAO.DeleteAsync(id);
    }
}
