using CoreEntities.Model;
using LogicBusiness.Repository;
using LogicBusiness.UseCase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.Service
{
    public class CustomFieldService : ICustomFieldService
    {
        private readonly ICustomFieldRepository _repo;

        public CustomFieldService(ICustomFieldRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<CustomFieldFL>> GetAllAsync() => _repo.GetAllAsync();

        public Task<CustomFieldFL?> GetByIdAsync(string id) => _repo.GetByIdAsync(id);

        public Task<CustomFieldFL> CreateAsync(CustomFieldFL entity)
        {
            entity.DateCreated = DateTime.UtcNow;
            return _repo.CreateAsync(entity);
        }

        public Task<CustomFieldFL> UpdateAsync(CustomFieldFL entity)
        {
            return _repo.UpdateAsync(entity);
        }

        public Task<bool> DeleteAsync(string id) => _repo.DeleteAsync(id);
    }
}
