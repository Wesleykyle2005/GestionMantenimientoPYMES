using System.ComponentModel.DataAnnotations;

namespace SharedModels.ModelsDTO.Usuario;

public class UsuarioUpdateDTO
{
    public int UsuarioId { get; set; }
    [Required, MaxLength(100)]
    public string Nombre { get; set; }

    [Required, EmailAddress, MaxLength(100)]
    public string Email { get; set; }

    [MaxLength(100)]
    public string Contraseña { get; set; } // Opcional para update

    [Required, MaxLength(50)]
    public string Rol { get; set; }

    [Required]
    public int EmpresaId { get; set; }
}