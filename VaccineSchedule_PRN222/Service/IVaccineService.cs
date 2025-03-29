using BussinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IVaccineService
    {
        Task<List<Vaccine>> GetAllAsync();
        Task<Vaccine?> GetByIdAsync(string id);
        Task AddAsync(Vaccine vaccine);
        Task UpdateAsync(Vaccine vaccine);
        Task DeleteAsync(string id);
    }
}
