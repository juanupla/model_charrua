using System.ComponentModel.DataAnnotations.Schema;

namespace Charrua_API.Models
{
    [Table("usuarios")]
    public class Usuario
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Authorization { get; set; }//que nivel de autorizacion posee : user o admin
        public bool Confirmado { get; set; } //confirmar user
        public string Token { get; set; }

        //public string Restablecer //este campo es para restablecer password

    }
}
