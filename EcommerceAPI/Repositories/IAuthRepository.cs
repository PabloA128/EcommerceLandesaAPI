using EcommerceAPI.Models;

namespace EcommerceAPI.Repositories
{
    public interface IAuthRepository
    {
        Task<Usuario> RegisterAsync(Usuario usuario);
        Task<Usuario?> GetUserByEmailAsync(string email);
        Task<Usuario?> GetUserByIdAsync(int id);
        Task<Usuario?> GetUserByResetTokenAsync(string token);
        Task<bool> UpdateUserAsync(Usuario usuario);
    }
}
