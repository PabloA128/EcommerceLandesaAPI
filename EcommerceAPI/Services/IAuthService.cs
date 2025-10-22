using EcommerceAPI.Models;

namespace EcommerceAPI.Services
{
    public interface IAuthService
    {
        Task<object> LoginAsync(string email, string password);
        Task<Usuario> RegisterAsync(Usuario usuario, string password);
        Task<bool> ForgotPasswordAsync(string email);
        Task<bool> ResetPasswordAsync(string token, string newPassword);
        Task<bool> CheckEmailAvailabilityAsync(string email);
        Task<Usuario?> GetUserByEmailAsync(string email);
        Task<Usuario?> GetUserByResetTokenAsync(string token);
        Task<bool> UpdatePasswordAsync(int userId, string newPassword);
    }
}
