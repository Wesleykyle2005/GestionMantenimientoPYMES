using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedModels.Models;

public class Usuario
{
    [Key]
    public int UsuarioId { get; set; }

    [Required, MaxLength(100)]
    public string Nombre { get; set; }

    [Required, MaxLength(100)]
    public string Email { get; set; }

    [Required]
    public byte[] Contraseña { get; set; } // SHA256 = 32 bytes

    [Required, MaxLength(50)]
    public string Rol { get; set; }

    [ForeignKey("Empresa")]
    public int EmpresaId { get; set; }
    public Empresa Empresa { get; set; }
}
