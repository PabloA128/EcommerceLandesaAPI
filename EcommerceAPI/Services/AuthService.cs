using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using EcommerceAPI.Models;
using EcommerceAPI.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;

namespace EcommerceAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IAuthRepository authRepository, IConfiguration configuration)
        {
            _authRepository = authRepository;
            _configuration = configuration;
        }

        public async Task<object> LoginAsync(string email, string password)
        {
            var user = await _authRepository.GetUserByEmailAsync(email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            var token = GenerateJwtToken(user);

            return new
            {
                success = true,
                message = "Login successful",
                token = token,
                user = new
                {
                    id = user.Id,
                    userType = user.UserType,
                    nombre = user.Nombre,
                    apellido = user.Apellido,
                    mail = user.Mail,
                    telefono = user.Telefono,
                    domicilio = user.Domicilio,
                    taller = user.Taller,
                    nombreTaller = user.NombreTaller
                }
            };
        }

        public async Task<Usuario> RegisterAsync(Usuario usuario, string password)
        {
            // Verificar si el email ya existe
            var existingUser = await _authRepository.GetUserByEmailAsync(usuario.Mail);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Email already exists");
            }

            // Hashear la contraseña
            usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
            usuario.FechaCreacion = DateTime.Now;

            return await _authRepository.RegisterAsync(usuario);
        }

        public async Task<bool> ForgotPasswordAsync(string email)
        {
            var user = await _authRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                return false; // No revelar si el email existe o no por seguridad
            }

            // NOTA: Como la tabla actual no tiene campos de reset token,
            // esta funcionalidad está simplificada.
            // Para implementar completamente, necesitarías agregar los campos:
            // ResetPasswordToken NVARCHAR(255) NULL
            // ResetPasswordExpires DATETIME2 NULL
            // a tu tabla Usuario
            
            // Por ahora solo retornamos true para simular que se envió el email
            return true;
        }

        public async Task<bool> ResetPasswordAsync(string token, string newPassword)
        {
            // NOTA: Como la tabla actual no tiene campos de reset token,
            // esta funcionalidad está deshabilitada por ahora.
            // Para implementar completamente, necesitarías agregar los campos:
            // ResetPasswordToken NVARCHAR(255) NULL
            // ResetPasswordExpires DATETIME2 NULL
            // a tu tabla Usuario
            
            return false; // Siempre retorna false hasta que se agreguen los campos necesarios
        }

        public async Task<bool> CheckEmailAvailabilityAsync(string email)
        {
            var user = await _authRepository.GetUserByEmailAsync(email);
            return user == null; // true si está disponible
        }

        public async Task<Usuario?> GetUserByEmailAsync(string email)
        {
            return await _authRepository.GetUserByEmailAsync(email);
        }

        public async Task<Usuario?> GetUserByResetTokenAsync(string token)
        {
            return await _authRepository.GetUserByResetTokenAsync(token);
        }

        public async Task<bool> UpdatePasswordAsync(int userId, string newPassword)
        {
            var user = await _authRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            user.FechaActualizacion = DateTime.Now;

            await _authRepository.UpdateUserAsync(user);
            return true;
        }

        private string GenerateJwtToken(Usuario user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Nombre),
                new Claim(ClaimTypes.Email, user.Mail),
                new Claim("UserType", user.UserType),
                new Claim("Taller", user.Taller.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateResetToken()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                var bytes = new byte[32];
                rng.GetBytes(bytes);
                return Convert.ToBase64String(bytes).Replace("+", "-").Replace("/", "_").Replace("=", "");
            }
        }
    }
}
