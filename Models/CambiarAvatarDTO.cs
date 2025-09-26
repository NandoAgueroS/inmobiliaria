using System.ComponentModel.DataAnnotations;

namespace inmobiliaria.Models
{
    public class CambiarAvatarDTO
    {
        [Required]
        public int IdUsuario { get; set; }
        [Required]
        public IFormFile AvatarFile { get; set; }
    }
}
