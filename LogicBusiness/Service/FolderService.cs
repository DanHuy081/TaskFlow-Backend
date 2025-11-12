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
    public class FolderService : IFolderService
    {
        private readonly IFolderRepository _repo;

        public FolderService(IFolderRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Folder>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Folder> GetByIdAsync(string id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task AddAsync(Folder folder)
        {
            folder.FolderId = Guid.NewGuid().ToString();
            folder.DateCreated = DateTime.UtcNow;
            await _repo.AddAsync(folder);
        }

        public async Task UpdateAsync(Folder folder)
        {
            folder.DateUpdated = DateTime.UtcNow;
            await _repo.UpdateAsync(folder);
        }

        public async Task DeleteAsync(string id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}
