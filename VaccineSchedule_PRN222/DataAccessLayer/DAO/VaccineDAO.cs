using BussinessLogicLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.DAO
{
    public class VaccineDAO
    {
        private readonly VaccineScheduleDbContext _context;

        public VaccineDAO()
        {
            _context = new VaccineScheduleDbContext();
        }

        // Lấy tất cả vắc xin
        public async Task<List<Vaccine>> GetAllAsync()
        {
            return await _context.Vaccines.Include(v => v.Schedules).ToListAsync();
        }

        // Lấy vắc xin theo ID
        public async Task<Vaccine?> GetByIdAsync(string id)
        {
            return await _context.Vaccines.Include(v => v.Schedules)
                                           .FirstOrDefaultAsync(v => v.Id == id);
        }

        // Thêm vắc xin mới
        public async Task AddAsync(Vaccine vaccine)
        {
            await _context.Vaccines.AddAsync(vaccine);
            await _context.SaveChangesAsync();
        }

        // Cập nhật thông tin vắc xin
        public async Task UpdateAsync(Vaccine vaccine)
        {
            _context.Vaccines.Update(vaccine);
            await _context.SaveChangesAsync();
        }

        // Xóa vắc xin theo ID
        public async Task DeleteAsync(string id)
        {
            var vaccine = await _context.Vaccines.FindAsync(id);
            if (vaccine != null)
            {
                _context.Vaccines.Remove(vaccine);
                await _context.SaveChangesAsync();
            }
        }
    }
}