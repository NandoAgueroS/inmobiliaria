using System.ComponentModel.DataAnnotations;

namespace inmobiliaria.Models
{
    public class UsuarioDTO
    {
        [Required(ErrorMessage = "El Email es obligatorio"), DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria"), DataType(DataType.Password)]
        public string Clave { get; set; }

        
    }
    

}