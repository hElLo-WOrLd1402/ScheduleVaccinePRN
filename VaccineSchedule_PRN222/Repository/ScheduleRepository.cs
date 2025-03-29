using BussinessLogicLayer;
using DataAccessLayer.DAO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly ScheduleDAO _scheduleDAO;

        public ScheduleRepository(VaccineScheduleDbContext context)
        {
            _scheduleDAO = new ScheduleDAO(context);
        }

        public async Task<List<Schedule>> GetAllAsync() =>
            await _scheduleDAO.GetAllAsync();

        public async Task<Schedule?> GetByIdAsync(string id) =>
            await _scheduleDAO.GetByIdAsync(id);

        public async Task AddAsync(Schedule schedule) =>
            await _scheduleDAO.AddAsync(schedule);

        public async Task UpdateAsync(Schedule schedule) =>
            await _scheduleDAO.UpdateAsync(schedule);

        public async Task DeleteAsync(string id) =>
            await _scheduleDAO.DeleteAsync(id);
    }
}
