using System.ComponentModel.DataAnnotations;

namespace SharedModels.ModelsDTO.Usuario;

public class UsuarioCreateDTO
{
    [Required, MaxLength(100)]
    public string Nombre { get; set; }

    [Required, EmailAddress, MaxLength(100)]
    public string Email { get; set; }

    [Required, MinLength(6), MaxLength(100)]
    public string Contraseña { get; set; } // En texto plano, la API debe hashearla

    [Required, MaxLength(50)]
    public string Rol { get; set; }

    [Required]
    public int EmpresaId { get; set; }
}
