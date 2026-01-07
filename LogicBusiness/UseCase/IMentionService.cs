using CoreEntities.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.UseCase
{
    public interface IMentionService
    {
        Task<List<MentionSuggestionDto>> SearchAsync(string keyword);
    }
}
