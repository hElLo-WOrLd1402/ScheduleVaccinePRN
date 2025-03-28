using BussinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
   public interface IChildrenProfileService
    {
        Task<List<ChildrenProfile>> GetAllChildrenProfilesAsync();
        Task<ChildrenProfile?> GetChildrenProfileByIdAsync(string id);
        Task<bool> AddChildrenProfileAsync(ChildrenProfile childrenProfile);
        Task<bool> UpdateChildrenProfileAsync(ChildrenProfile childrenProfile);
        Task<bool> DeleteChildrenProfileAsync(string id);
    }
}
