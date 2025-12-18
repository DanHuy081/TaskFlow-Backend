using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.UseCase
{
    public interface ISummaryService
    {
        Task<string?> GetSummaryAsync(Guid conversationId);
        Task UpdateSummaryIfNeededAsync(Guid conversationId);
    }
}
