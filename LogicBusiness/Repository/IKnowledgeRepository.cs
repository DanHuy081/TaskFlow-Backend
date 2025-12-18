using CoreEntities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.Repository
{
    public interface IKnowledgeRepository
    {
        Task<List<KnowledgeChunk>> SearchAsync(string query, int take = 5);
        Task<List<KnowledgeChunk>> SearchByTagsAsync(IEnumerable<string> tags, int take = 5);
    }
}
