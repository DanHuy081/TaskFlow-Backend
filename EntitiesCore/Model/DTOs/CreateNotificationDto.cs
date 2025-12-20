using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model.DTOs
{
    public class CreateNotificationDto
    {
        public Guid UserId { get; set; } // int -> Guid
        public string Title { get; set; }
        public string Message { get; set; }
    }
}
