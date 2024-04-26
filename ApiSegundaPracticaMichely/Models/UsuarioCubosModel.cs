using System.ComponentModel.DataAnnotations.Schema;

namespace ApiSegundaPracticaMichely.Models
{
    public class UsuarioCubosModel
    {
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Imagen { get; set; }
    }
}
