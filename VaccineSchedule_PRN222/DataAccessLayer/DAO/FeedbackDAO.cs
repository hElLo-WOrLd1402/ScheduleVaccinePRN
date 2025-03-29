using BussinessLogicLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.DAO
{
    public class FeedbackDAO
    {
        private readonly VaccineScheduleDbContext _context;

        public FeedbackDAO(VaccineScheduleDbContext context)
        {
            _context = context;
        }

        // Lấy tất cả phản hồi
        public async Task<List<Feedback>> GetAllAsync()
        {
            return await _context.Feedbacks.Include(f => f.User)
                                           .Include(f => f.Schedule)
                                           .ToListAsync();
        }

        // Lấy phản hồi theo ID
        public async Task<Feedback?> GetByIdAsync(string id)
        {
            return await _context.Feedbacks.Include(f => f.User)
                                           .Include(f => f.Schedule)
                                           .FirstOrDefaultAsync(f => f.Id == id);
        }

        // Thêm phản hồi mới
        public async Task AddAsync(Feedback feedback)
        {
            await _context.Feedbacks.AddAsync(feedback);
            await _context.SaveChangesAsync();
        }

        // Cập nhật thông tin phản hồi
        public async Task UpdateAsync(Feedback feedback)
        {
            _context.Feedbacks.Update(feedback);
            await _context.SaveChangesAsync();
        }

        // Xóa phản hồi theo ID
        public async Task DeleteAsync(string id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback != null)
            {
                _context.Feedbacks.Remove(feedback);
                await _context.SaveChangesAsync();
            }
        }
    }
}
