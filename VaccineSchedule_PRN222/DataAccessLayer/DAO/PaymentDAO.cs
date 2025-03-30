using BussinessLogicLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.DAO
{
    public class PaymentDAO
    {       

        // Lấy tất cả các thanh toán
        public async Task<List<Payment>> GetAllPaymentAsync()
        {
            using var db = new VaccineScheduleDbContext();
            return await db.Payments.Include(p => p.Schedule).ToListAsync();
        }

        // Lấy thanh toán theo ID
        public async Task<Payment?> GetPaymentByIdAsync(string id)
        {
            using var db = new VaccineScheduleDbContext();
            return await db.Payments.Include(p => p.Schedule)
                                          .FirstOrDefaultAsync(p => p.Id == id);
        }

        // Thêm thanh toán mới
        public async Task AddAsync(Payment payment)
        {
            using var db = new VaccineScheduleDbContext();
            await db.Payments.AddAsync(payment);
            await db.SaveChangesAsync();
        }

        // Cập nhật thông tin thanh toán
        public async Task UpdateAsync(Payment payment)
        {
            using var db = new VaccineScheduleDbContext();
            db.Payments.Update(payment);
            await db.SaveChangesAsync();
        }

        // Xóa thanh toán theo ID
        public async Task DeletePaymentAsync(string id)
        {
            using var db = new VaccineScheduleDbContext();
            var payment = await db.Payments.FindAsync(id);
            if (payment != null)
            {
                db.Payments.Remove(payment);
                await db.SaveChangesAsync();
            }
        }

        // Lấy danh sách thanh toán có trạng thái "Pending"
        public async Task<List<Payment>> GetPendingPaymentsAsync()
        {
            using var db = new VaccineScheduleDbContext();
            return await db.Payments.Include(p => p.Schedule)
                                          .Where(p => p.PaymentStatus == "Pending")
                                          .ToListAsync();
        }
    }
}