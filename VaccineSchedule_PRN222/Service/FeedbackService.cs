using BussinessLogicLayer;
using Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;

        public FeedbackService(IFeedbackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
        }

        public async Task<List<Feedback>> GetAllFeedbacksAsync() =>
            await _feedbackRepository.GetAllAsync();

        public async Task<Feedback?> GetFeedbackByIdAsync(string id) =>
            await _feedbackRepository.GetByIdAsync(id);

        public async Task<bool> AddFeedbackAsync(Feedback feedback)
        {
            if (string.IsNullOrEmpty(feedback.UserId) || string.IsNullOrEmpty(feedback.ScheduleId))
                return false;

            await _feedbackRepository.AddAsync(feedback);
            return true;
        }

        public async Task<bool> UpdateFeedbackAsync(Feedback feedback)
        {
            if (await _feedbackRepository.GetByIdAsync(feedback.Id) == null)
                return false;

            await _feedbackRepository.UpdateAsync(feedback);
            return true;
        }

        public async Task<bool> DeleteFeedbackAsync(string id)
        {
            if (await _feedbackRepository.GetByIdAsync(id) == null)
                return false;

            await _feedbackRepository.DeleteAsync(id);
            return true;
        }
    }
}
