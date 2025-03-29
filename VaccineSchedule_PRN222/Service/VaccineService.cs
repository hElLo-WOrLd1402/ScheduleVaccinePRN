using BussinessLogicLayer;
using DataAccessLayer.DAO;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class VaccineService : IVaccineService
    {
        private readonly IVaccineRepository _vaccineRepository;

        public VaccineService(IVaccineRepository vaccineRepository)
        {
            _vaccineRepository = vaccineRepository;
        }
        public async Task<List<Vaccine>> GetAllAsync() =>
            await _vaccineRepository.GetAllAsync();

        public async Task<Vaccine?> GetByIdAsync(string id) =>
            await _vaccineRepository.GetByIdAsync(id);

        public async Task AddAsync(Vaccine vaccine) =>
            await _vaccineRepository.AddAsync(vaccine);

        public async Task UpdateAsync(Vaccine vaccine) =>
            await _vaccineRepository.UpdateAsync(vaccine);

        public async Task DeleteAsync(string id) =>
            await _vaccineRepository.DeleteAsync(id);
    }

}
