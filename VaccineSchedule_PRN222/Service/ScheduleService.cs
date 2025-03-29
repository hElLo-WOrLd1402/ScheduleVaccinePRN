using BussinessLogicLayer;
using Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service
{
    public class ScheduleService : IScheduleService
    {
        private readonly IScheduleRepository _scheduleRepository;

        public ScheduleService(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }

        public async Task<List<Schedule>> GetAllSchedulesAsync() =>
            await _scheduleRepository.GetAllAsync();

        public async Task<Schedule?> GetScheduleByIdAsync(string id) =>
            await _scheduleRepository.GetByIdAsync(id);

        public async Task<bool> AddScheduleAsync(Schedule schedule)
        {
            if (string.IsNullOrEmpty(schedule.ChildId) || string.IsNullOrEmpty(schedule.VaccineId))
                return false;

            await _scheduleRepository.AddAsync(schedule);
            return true;
        }

        public async Task<bool> UpdateScheduleAsync(Schedule schedule)
        {
            if (await _scheduleRepository.GetByIdAsync(schedule.Id) == null)
                return false;

            await _scheduleRepository.UpdateAsync(schedule);
            return true;
        }

        public async Task<bool> DeleteScheduleAsync(string id)
        {
            if (await _scheduleRepository.GetByIdAsync(id) == null)
                return false;

            await _scheduleRepository.DeleteAsync(id);
            return true;
        }
    }
}
