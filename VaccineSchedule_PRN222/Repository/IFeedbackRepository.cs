using BussinessLogicLayer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository
{
    public interface IFeedbackRepository
    {
        Task<List<Feedback>> GetAllAsync();
        Task<Feedback?> GetByIdAsync(string id);
        Task AddAsync(Feedback feedback);
        Task UpdateAsync(Feedback feedback);
        Task DeleteAsync(string id);
    }
}
