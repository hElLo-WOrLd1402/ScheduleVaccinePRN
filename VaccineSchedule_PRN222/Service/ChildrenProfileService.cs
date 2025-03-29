using BussinessLogicLayer;
using Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service
{
    public class ChildrenProfileService : IChildrenProfileService
    {
        private readonly IChildrenProfileRepository _childrenProfileRepository;
      
        public ChildrenProfileService(IChildrenProfileRepository childrenProfileRepository)
        {
            _childrenProfileRepository = childrenProfileRepository;
        }

        public async Task<List<ChildrenProfile>> GetAllChildrenProfilesAsync() =>
            await _childrenProfileRepository.GetAllAsync();

        public async Task<ChildrenProfile?> GetChildrenProfileByIdAsync(string id) =>
            await _childrenProfileRepository.GetByIdAsync(id);

        public async Task<bool> AddChildrenProfileAsync(ChildrenProfile childrenProfile)
        {
            if (string.IsNullOrEmpty(childrenProfile.ParentId))
                return false;

            await _childrenProfileRepository.AddAsync(childrenProfile);
            return true;
        }

        public async Task<bool> UpdateChildrenProfileAsync(ChildrenProfile childrenProfile)
        {
            if (await _childrenProfileRepository.GetByIdAsync(childrenProfile.Id) == null)
                return false;

            await _childrenProfileRepository.UpdateAsync(childrenProfile);
            return true;
        }

        public async Task<bool> DeleteChildrenProfileAsync(string id)
        {
            if (await _childrenProfileRepository.GetByIdAsync(id) == null)
                return false;

            await _childrenProfileRepository.DeleteAsync(id);
            return true;
        }
    }
}
