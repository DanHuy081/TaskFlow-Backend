using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model.DTOs
{
    public class ForgotPasswordRequestDto
    {
        public string Email { get; set; }
    }

    public class ResetPasswordRequestDto
    {
        public Guid Token { get; set; }
        public string NewPassword { get; set; }
    }

}
