using CoreEntities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.Repository
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserFL>> GetAllAsync();
        Task<UserFL> GetByIdAsync(string id);
        Task AddAsync(UserFL user);
        Task UpdateAsync(UserFL user);
        Task DeleteAsync(string id);

        Task<UserFL?> GetByUsernameAsync(string username);
        Task<UserFL?> GetByEmailAsync(string email);
        Task CreateAsync(UserFL user);

       
        Task<UserFL> GetByResetTokenAsync(Guid token);
        
    }
}
