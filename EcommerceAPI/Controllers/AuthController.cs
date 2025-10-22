using Microsoft.AspNetCore.Mvc;
using EcommerceAPI.Models;
using EcommerceAPI.Services;

namespace EcommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // Endpoint para login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            try
            {
                var result = await _authService.LoginAsync(model.Email, model.Password);
                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Ok(new { success = false, message = "Invalid credentials" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Internal server error" });
            }
        }

        // Endpoint para registro
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            try
            {

                // Mapear la solicitud de registro al modelo Usuario
                var usuario = new Usuario
                {
                    Nombre = model.UserType == "user" ? model.FirstName : model.WorkshopName,
                    Apellido = model.UserType == "user" ? model.LastName : "",
                    Mail = model.UserType == "user" ? model.Email : model.WorkshopEmail,
                    Telefono = model.UserType == "user" ? model.Phone : model.WorkshopPhone,
                    Domicilio = model.UserType == "user" ? model.Address : model.WorkshopAddress,
                    Taller = model.UserType == "workshop", // UserType se calcula automáticamente basado en este campo
                    NombreTaller = model.UserType == "workshop" ? model.WorkshopName : null
                };
                
                var password = model.UserType == "user" ? model.Password : model.WorkshopPassword;
                var createdUser = await _authService.RegisterAsync(usuario, password);

                // Generar token para el usuario recién registrado
                var loginResult = await _authService.LoginAsync(createdUser.Mail, password);

                return Ok(loginResult);
            }
            catch (InvalidOperationException ex)
            {
                return Ok(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Internal server error" });
            }
        }

        // Endpoint para solicitar reset de contraseña
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest model)
        {
            try
            {
                var result = await _authService.ForgotPasswordAsync(model.Email);
                // Siempre retornamos éxito por seguridad, sin revelar si el email existe
                return Ok(new { success = true, message = "If the email exists, a reset link has been sent" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Internal server error" });
            }
        }

        // Endpoint para resetear contraseña
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest model)
        {
            try
            {
                var result = await _authService.ResetPasswordAsync(model.Token, model.NewPassword);
                if (result)
                {
                    return Ok(new { success = true, message = "Password reset successfully" });
                }
                else
                {
                    return Ok(new { success = false, message = "Invalid or expired token" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Internal server error" });
            }
        }

        // Endpoint para verificar disponibilidad de email
        [HttpGet("check-email")]
        public async Task<IActionResult> CheckEmailAvailability([FromQuery] string email)
        {
            try
            {
                var isAvailable = await _authService.CheckEmailAvailabilityAsync(email);
                return Ok(isAvailable);
            }
            catch (Exception ex)
            {
                return StatusCode(500, false);
            }
        }
    }

    // DTOs para las solicitudes
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RegisterRequest
    {
        public string UserType { get; set; }
        // Campos para usuario normal
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        // Campos para taller
        public string? WorkshopName { get; set; }
        public string? WorkshopEmail { get; set; }
        public string? WorkshopPhone { get; set; }
        public string? WorkshopAddress { get; set; }
        public string? WorkshopPassword { get; set; }
        public string? WorkshopConfirmPassword { get; set; }
    }

    public class ForgotPasswordRequest
    {
        public string Email { get; set; }
    }

    public class ResetPasswordRequest
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
