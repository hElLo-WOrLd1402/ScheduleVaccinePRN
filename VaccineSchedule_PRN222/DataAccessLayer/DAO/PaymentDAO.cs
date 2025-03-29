using BussinessLogicLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.DAO
{
    public class PaymentDAO
    {
        private readonly VaccineScheduleDbContext _context;

        public PaymentDAO(VaccineScheduleDbContext context)
        {
            _context = context;
        }

        // Lấy tất cả các thanh toán
        public async Task<List<Payment>> GetAllAsync()
        {
            return await _context.Payments.Include(p => p.Schedule).ToListAsync();
        }

        // Lấy thanh toán theo ID
        public async Task<Payment?> GetByIdAsync(string id)
        {
            return await _context.Payments.Include(p => p.Schedule)
                                          .FirstOrDefaultAsync(p => p.Id == id);
        }

        // Thêm thanh toán mới
        public async Task AddAsync(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();
        }

        // Cập nhật thông tin thanh toán
        public async Task UpdateAsync(Payment payment)
        {
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
        }

        // Xóa thanh toán theo ID
        public async Task DeleteAsync(string id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment != null)
            {
                _context.Payments.Remove(payment);
                await _context.SaveChangesAsync();
            }
        }
    }
}
