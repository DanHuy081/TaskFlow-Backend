using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model.DTOs
{
    public class UserMiniDto
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string ProfilePicture { get; set; }
        public string Color { get; set; }   // màu avatar
    }

}
