using BussinessLogicLayer;
using DataAccessLayer.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class VaccineRepository : IVaccineRepository
    {
        private readonly VaccineDAO _vaccineDAO;

        public VaccineRepository()
        {
            _vaccineDAO = new VaccineDAO();
        }
        public async Task<List<Vaccine>> GetAllAsync() =>
            await _vaccineDAO.GetAllAsync();

        public async Task<Vaccine?> GetByIdAsync(string id) =>
            await _vaccineDAO.GetByIdAsync(id);

        public async Task AddAsync(Vaccine vaccine) =>
            await _vaccineDAO.AddAsync(vaccine);

        public async Task UpdateAsync(Vaccine vaccine) =>
            await _vaccineDAO.UpdateAsync(vaccine);

        public async Task DeleteAsync(string id) =>
            await _vaccineDAO.DeleteAsync(id);
    }
}
