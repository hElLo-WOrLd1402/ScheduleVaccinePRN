using BussinessLogicLayer;
using DataAccessLayer.DAO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository
{
    public class ChildrenProfileRepository : IChildrenProfileRepository
    {
        private readonly ChildrenProfileDAO _childrenProfileDAO;

        // Inject DAO thông qua constructor
        public ChildrenProfileRepository()
        {
            _childrenProfileDAO = new ChildrenProfileDAO();
        }

        public async Task<List<ChildrenProfile>> GetAllAsync() =>
            await _childrenProfileDAO.GetAllAsync();

        public async Task<ChildrenProfile?> GetByIdAsync(string id) =>
            await _childrenProfileDAO.GetByIdAsync(id);

        public async Task AddAsync(ChildrenProfile childrenProfile) =>
            await _childrenProfileDAO.AddAsync(childrenProfile);

        public async Task UpdateAsync(ChildrenProfile childrenProfile) =>
            await _childrenProfileDAO.UpdateAsync(childrenProfile);

        public async Task DeleteAsync(string id) =>
            await _childrenProfileDAO.DeleteAsync(id);
    }
}
