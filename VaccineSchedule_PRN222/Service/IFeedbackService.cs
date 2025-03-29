using BussinessLogicLayer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service
{
    public interface IFeedbackService
    {
        Task<List<Feedback>> GetAllFeedbacksAsync();
        Task<Feedback?> GetFeedbackByIdAsync(string id);
        Task<bool> AddFeedbackAsync(Feedback feedback);
        Task<bool> UpdateFeedbackAsync(Feedback feedback);
        Task<bool> DeleteFeedbackAsync(string id);
    }
}
