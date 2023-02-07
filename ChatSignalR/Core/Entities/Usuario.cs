using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ChatSignalR.Core.Entities
{
    [Table("Usuarios")]
    public class Usuario
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("Nombre")]
        public string Nombre { get; set; }

        [Column("Apellido")]
        public string Apellidos { get; set; }

        [Column("Email")]
        public string Email { get; set; }

        [Column("Password")]
        public byte[] Password { get; set; }

        [Column("Salt")]
        public string Salt { get; set; }

        [Column("Tipo")]
        public string Tipo { get; set; }
    }
}
