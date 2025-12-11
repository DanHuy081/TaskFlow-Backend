using CoreEntities.Model;
using LogicBusiness.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlServer.Data;

namespace SqlServer
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserFL>> GetAllAsync()
        {
            return await _context.UserFLs.ToListAsync();
        }

        public async Task<UserFL> GetByIdAsync(string id)
        {
            return await _context.UserFLs.FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task AddAsync(UserFL user)
        {
            _context.UserFLs.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(UserFL user)
        {
            _context.UserFLs.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var user = await _context.UserFLs.FindAsync(id);
            if (user != null)
            {
                _context.UserFLs.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<UserFL?> GetByUsernameAsync(string username)
        => await _context.UserFLs.FirstOrDefaultAsync(u => u.Username == username);

        public async Task<UserFL?> GetByEmailAsync(string email)
            => await _context.UserFLs.FirstOrDefaultAsync(u => u.Email == email);

        public async Task CreateAsync(UserFL user)
        {
            _context.UserFLs.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}
