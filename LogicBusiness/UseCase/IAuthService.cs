using CoreEntities.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.UseCase
{
    public interface IAuthService
    {
        Task<string?> LoginAsync(LoginDto dto);
        Task<bool> RegisterAsync(RegisterDto dto);

        Task ForgotPasswordAsync(string email);
        Task ResetPasswordAsync(Guid token, string newPassword);

        Task<bool> ChangePasswordAsync(string userId, ChangePasswordDto dto);
    }
}


