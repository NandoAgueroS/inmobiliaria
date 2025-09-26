using System.ComponentModel.DataAnnotations;


namespace inmobiliaria.Models {
    
    public class Propietario : Persona
    {
        [Required(ErrorMessage = "La dirección es obligatoria")]
        public string Direccion { get; set; }
    }
}


