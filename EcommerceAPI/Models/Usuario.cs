namespace EcommerceAPI.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Mail { get; set; }
        public string Contrasena { get; set; } // Campo que existe en tu tabla
        public string? Telefono { get; set; }
        public string? Domicilio { get; set; }
        public bool Taller { get; set; }
        public string? NombreTaller { get; set; }
        
        // Propiedades adicionales que no están en la tabla pero necesitamos para la lógica
        // Estas se manejarán en memoria o se pueden agregar a la tabla más adelante
        public string UserType => Taller ? "taller" : "normal";
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime? FechaActualizacion { get; set; }
        public string? ResetPasswordToken { get; set; }
        public DateTime? ResetPasswordExpires { get; set; }
        
        // Propiedad de conveniencia para compatibilidad con el servicio
        public string PasswordHash 
        { 
            get => Contrasena; 
            set => Contrasena = value; 
        }
    }
}
