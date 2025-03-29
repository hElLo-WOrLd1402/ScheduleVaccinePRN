using BussinessLogicLayer;
using Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<List<Payment>> GetAllPaymentsAsync() =>
            await _paymentRepository.GetAllAsync();

        public async Task<Payment?> GetPaymentByIdAsync(string id) =>
            await _paymentRepository.GetByIdAsync(id);

        public async Task<bool> AddPaymentAsync(Payment payment)
        {
            if (string.IsNullOrEmpty(payment.Id) || payment.Amount <= 0)
                return false;

            await _paymentRepository.AddAsync(payment);
            return true;
        }

        public async Task<bool> UpdatePaymentAsync(Payment payment)
        {
            if (await _paymentRepository.GetByIdAsync(payment.Id) == null)
                return false;

            await _paymentRepository.UpdateAsync(payment);
            return true;
        }

        public async Task<bool> DeletePaymentAsync(string id)
        {
            if (await _paymentRepository.GetByIdAsync(id) == null)
                return false;

            await _paymentRepository.DeleteAsync(id);
            return true;
        }
    }
}
