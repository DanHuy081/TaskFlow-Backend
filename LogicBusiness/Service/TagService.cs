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
    public class TagService : ITagService
    {
        private readonly ITagRepository _repo;

        public TagService(ITagRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Tag>> GetAllTagsAsync() => await _repo.GetAllAsync();
        public async Task<Tag> GetTagByIdAsync(string id) => await _repo.GetByIdAsync(id);

        public async Task AddTagAsync(Tag tag)
        {
            tag.TagId = Guid.NewGuid().ToString();
            tag.DateCreated = DateTime.UtcNow;
            await _repo.AddAsync(tag);
        }

        public async Task UpdateTagAsync(Tag tag)
        {
            await _repo.UpdateAsync(tag);
        }

        public async Task DeleteTagAsync(string id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}
