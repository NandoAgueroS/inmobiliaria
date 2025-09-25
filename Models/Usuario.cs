using System.ComponentModel.DataAnnotations;

namespace inmobiliaria.Models
{
    public enum enRoles
    {
        Administrador = 1,
        Empleado = 2,
    }
    public class Usuario
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }


        [Required(ErrorMessage = "El Apellido es obligatorio")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El Email es obligatorio"), EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria"), DataType(DataType.Password)]
        public string Clave { get; set; }

        public string? Avatar { get; set; }
        public IFormFile? AvatarFile { get; set; }

        [Required(ErrorMessage = "El Rol es obligatorio")]
        public int Rol { get; set; }
        public bool Estado { get; set; }

        public string RolNombre => Rol > 0 ? ((enRoles)Rol).ToString() : "";
        public static IDictionary<int, string> ObtenerRoles()
        {
            SortedDictionary<int, string> roles = new SortedDictionary<int, string>();
            Type tipoEnumRol = typeof(enRoles);
            foreach (var valor in Enum.GetValues(tipoEnumRol))
            {
                roles.Add((int)valor, Enum.GetName(tipoEnumRol, valor));
            }
            return roles;
        }

        
        
        
    }
}