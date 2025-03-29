using BussinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IChildrenProfileRepository
    {
        Task<List<ChildrenProfile>> GetAllAsync();
        Task<ChildrenProfile?> GetByIdAsync(string id);
        Task AddAsync(ChildrenProfile childrenProfile);
        Task UpdateAsync(ChildrenProfile childrenProfile);
        Task DeleteAsync(string id);
    }
}
