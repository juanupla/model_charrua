using System.ComponentModel.DataAnnotations.Schema;

namespace Charrua_API.Models
{
    [Table("usuarios")]
    public class Usuario
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Authorization { get; set; }

    }
}
