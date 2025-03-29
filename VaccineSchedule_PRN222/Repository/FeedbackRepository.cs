using BussinessLogicLayer;
using DataAccessLayer.DAO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository
{
    public class FeedbackRepository : IFeedbackRepository
    {
       
        private readonly FeedbackDAO _FeedbackDAO;

        public FeedbackRepository(FeedbackDAO FeedbackDAO)
        {
            _FeedbackDAO = FeedbackDAO;
        }


        public async Task<List<Feedback>> GetAllAsync() =>
            await _FeedbackDAO.GetAllAsync();

        public async Task<Feedback?> GetByIdAsync(string id) =>
            await _FeedbackDAO.GetByIdAsync(id);

        public async Task AddAsync(Feedback Feedback) =>
            await _FeedbackDAO.AddAsync(Feedback);

        public async Task UpdateAsync(Feedback Feedback) =>
            await _FeedbackDAO.UpdateAsync(Feedback);

        public async Task DeleteAsync(string id) =>
            await _FeedbackDAO.DeleteAsync(id);
    }
}
