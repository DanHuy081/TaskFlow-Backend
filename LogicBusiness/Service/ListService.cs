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
    public class ListService : IListService
    {
        private readonly IListRepository _repo;

        public ListService(IListRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<ListFL>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<ListFL> GetByIdAsync(string id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task AddAsync(ListFL list)
        {
            list.ListId = Guid.NewGuid().ToString();
            list.DateCreated = DateTime.UtcNow;
            await _repo.AddAsync(list);
        }

        public async Task UpdateAsync(ListFL list)
        {
            list.DateUpdated = DateTime.UtcNow;
            await _repo.UpdateAsync(list);
        }

        public async Task DeleteAsync(string id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}
