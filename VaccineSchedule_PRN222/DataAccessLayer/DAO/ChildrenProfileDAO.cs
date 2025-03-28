using BussinessLogicLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.DAO
{
    public class ChildrenProfileDAO
    {
        private readonly VaccineScheduleDbContext _context;

        public ChildrenProfileDAO()
        {
            _context = new VaccineScheduleDbContext();
        }

        // Lấy tất cả hồ sơ trẻ em
        public async Task<List<ChildrenProfile>> GetAllAsync()
        {
            return await _context.ChildrenProfiles.Include(c => c.Parent)
                                                  .Include(c => c.Schedules)
                                                  .ToListAsync();
        }

        // Lấy hồ sơ trẻ em theo ID
        public async Task<ChildrenProfile?> GetByIdAsync(string id)
        {
            return await _context.ChildrenProfiles.Include(c => c.Parent)
                                                  .Include(c => c.Schedules)
                                                  .FirstOrDefaultAsync(c => c.Id == id);
        }

        // Thêm hồ sơ trẻ em
        public async Task AddAsync(ChildrenProfile childrenProfile)
        {
            await _context.ChildrenProfiles.AddAsync(childrenProfile);
            await _context.SaveChangesAsync();
        }

        // Cập nhật hồ sơ trẻ em
        public async Task UpdateAsync(ChildrenProfile childrenProfile)
        {
            _context.ChildrenProfiles.Update(childrenProfile);
            await _context.SaveChangesAsync();
        }

        // Xóa hồ sơ trẻ em
        public async Task DeleteAsync(string id)
        {
            var profile = await _context.ChildrenProfiles.FindAsync(id);
            if (profile != null)
            {
                _context.ChildrenProfiles.Remove(profile);
                await _context.SaveChangesAsync();
            }
        }
    }
}
