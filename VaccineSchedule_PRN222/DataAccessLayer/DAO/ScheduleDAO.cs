using BussinessLogicLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.DAO
{
    public class ScheduleDAO
    {
        private readonly VaccineScheduleDbContext _context;

        public ScheduleDAO(VaccineScheduleDbContext context)
        {
            _context = context;
        }

        // Lấy tất cả lịch hẹn
        public async Task<List<Schedule>> GetAllAsync()
        {
            return await _context.Schedules.Include(s => s.Child)
                                           .Include(s => s.Vaccine)
                                           .Include(s => s.Feedbacks)
                                           .Include(s => s.Payments)
                                           .ToListAsync();
        }

        // Lấy lịch hẹn theo ID
        public async Task<Schedule?> GetByIdAsync(string id)
        {
            return await _context.Schedules.Include(s => s.Child)
                                           .Include(s => s.Vaccine)
                                           .Include(s => s.Feedbacks)
                                           .Include(s => s.Payments)
                                           .FirstOrDefaultAsync(s => s.Id == id);
        }

        // Thêm lịch hẹn mới
        public async Task AddAsync(Schedule schedule)
        {
            await _context.Schedules.AddAsync(schedule);
            await _context.SaveChangesAsync();
        }

        // Cập nhật thông tin lịch hẹn
        public async Task UpdateAsync(Schedule schedule)
        {
            _context.Schedules.Update(schedule);
            await _context.SaveChangesAsync();
        }

        // Xóa lịch hẹn theo ID
        public async Task DeleteAsync(string id)
        {
            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule != null)
            {
                _context.Schedules.Remove(schedule);
                await _context.SaveChangesAsync();
            }
        }
    }
}
