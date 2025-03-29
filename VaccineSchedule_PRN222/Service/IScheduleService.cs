using BussinessLogicLayer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service
{
    public interface IScheduleService
    {
        Task<List<Schedule>> GetAllSchedulesAsync();
        Task<Schedule?> GetScheduleByIdAsync(string id);
        Task<bool> AddScheduleAsync(Schedule schedule);
        Task<bool> UpdateScheduleAsync(Schedule schedule);
        Task<bool> DeleteScheduleAsync(string id);
    }
}
