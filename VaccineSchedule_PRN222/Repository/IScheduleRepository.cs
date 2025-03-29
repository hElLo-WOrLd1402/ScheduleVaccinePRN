using BussinessLogicLayer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository
{
    public interface IScheduleRepository
    {
        Task<List<Schedule>> GetAllAsync();
        Task<Schedule?> GetByIdAsync(string id);
        Task AddAsync(Schedule schedule);
        Task UpdateAsync(Schedule schedule);
        Task DeleteAsync(string id);
    }
}
