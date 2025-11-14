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
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;

        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<UserFL>> GetAllAsync()
            => await _repo.GetAllAsync();

        public async Task<UserFL> GetByIdAsync(string id)
            => await _repo.GetByIdAsync(id);

        public async Task AddAsync(UserFL user)
        {
            user.UserId = Guid.NewGuid().ToString();
            user.DateCreated = DateTime.UtcNow;
            await _repo.AddAsync(user);
        }

        public async Task UpdateAsync(UserFL user)
        {
            user.DateUpdated = DateTime.UtcNow;
            await _repo.UpdateAsync(user);
        }

        public async Task DeleteAsync(string id)
            => await _repo.DeleteAsync(id);
    }
}
