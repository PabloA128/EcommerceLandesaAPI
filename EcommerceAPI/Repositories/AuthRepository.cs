using EcommerceAPI.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EcommerceAPI.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly string _connectionString;

        public AuthRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<Usuario> RegisterAsync(Usuario usuario)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = @"
                INSERT INTO Usuario (Nombre, Apellido, Mail, Telefono, Domicilio, Contrasena, Taller, NombreTaller)
                VALUES (@Nombre, @Apellido, @Mail, @Telefono, @Domicilio, @Contrasena, @Taller, @NombreTaller);
                SELECT CAST(SCOPE_IDENTITY() as int);";

            using var command = new SqlCommand(query, connection);
            
            // Debug: Log de los valores que se van a insertar
            Console.WriteLine($"Insertando en BD - Nombre: '{usuario.Nombre}', Apellido: '{usuario.Apellido}'");
            Console.WriteLine($"Mail: '{usuario.Mail}', Telefono: '{usuario.Telefono}'");
            Console.WriteLine($"Domicilio: '{usuario.Domicilio}', Contrasena: '{(string.IsNullOrEmpty(usuario.Contrasena) ? "VACIA" : "PRESENTE")}'");
            Console.WriteLine($"Taller: {usuario.Taller}, NombreTaller: '{usuario.NombreTaller}'");
            
            command.Parameters.AddWithValue("@Nombre", usuario.Nombre ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Apellido", usuario.Apellido ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Mail", usuario.Mail ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Telefono", usuario.Telefono ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Domicilio", usuario.Domicilio ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Contrasena", usuario.Contrasena ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Taller", usuario.Taller);
            command.Parameters.AddWithValue("@NombreTaller", usuario.NombreTaller ?? (object)DBNull.Value);

            var id = await command.ExecuteScalarAsync();
            usuario.Id = Convert.ToInt32(id);

            return usuario;
        }

        public async Task<Usuario?> GetUserByEmailAsync(string email)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = @"
                SELECT Id, Nombre, Apellido, Mail, Telefono, Domicilio, Contrasena, Taller, NombreTaller
                FROM Usuario 
                WHERE Mail = @Email";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Email", email);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapUsuario(reader);
            }

            return null;
        }

        public async Task<Usuario?> GetUserByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = @"
                SELECT Id, Nombre, Apellido, Mail, Telefono, Domicilio, Contrasena, Taller, NombreTaller
                FROM Usuario 
                WHERE Id = @Id";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapUsuario(reader);
            }

            return null;
        }

        public async Task<Usuario?> GetUserByResetTokenAsync(string token)
        {
            // Como la tabla actual no tiene campos de reset token, 
            // este método retorna null por ahora
            // Se puede implementar más adelante agregando los campos a la tabla
            return null;
        }

        public async Task<bool> UpdateUserAsync(Usuario usuario)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = @"
                UPDATE Usuario 
                SET Nombre = @Nombre, Apellido = @Apellido, Mail = @Mail, Telefono = @Telefono, 
                    Domicilio = @Domicilio, Contrasena = @Contrasena, Taller = @Taller, 
                    NombreTaller = @NombreTaller
                WHERE Id = @Id";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", usuario.Id);
            command.Parameters.AddWithValue("@Nombre", usuario.Nombre);
            command.Parameters.AddWithValue("@Apellido", usuario.Apellido);
            command.Parameters.AddWithValue("@Mail", usuario.Mail);
            command.Parameters.AddWithValue("@Telefono", usuario.Telefono ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Domicilio", usuario.Domicilio ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Contrasena", usuario.Contrasena);
            command.Parameters.AddWithValue("@Taller", usuario.Taller);
            command.Parameters.AddWithValue("@NombreTaller", usuario.NombreTaller ?? (object)DBNull.Value);

            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        private Usuario MapUsuario(SqlDataReader reader)
        {
            return new Usuario
            {
                Id = reader.GetInt32("Id"),
                Nombre = reader.GetString("Nombre"),
                Apellido = reader.GetString("Apellido"),
                Mail = reader.GetString("Mail"),
                Telefono = reader.IsDBNull("Telefono") ? null : reader.GetString("Telefono"),
                Domicilio = reader.IsDBNull("Domicilio") ? null : reader.GetString("Domicilio"),
                Contrasena = reader.GetString("Contrasena"),
                Taller = reader.GetBoolean("Taller"),
                NombreTaller = reader.IsDBNull("NombreTaller") ? null : reader.GetString("NombreTaller")
            };
        }
    }
}
