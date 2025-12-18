using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model.DTOs
{
    public class ListBriefDto
    {
        public string ListId { get; set; } = "";
        public string SpaceId { get; set; } = "";   // nếu list thuộc space trực tiếp
        public string Name { get; set; } = "";
    }

}
