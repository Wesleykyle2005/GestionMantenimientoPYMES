using System.ComponentModel.DataAnnotations;

namespace SharedModels.ModelsDTO.Usuario;

public class UsuarioLoginDTO
{
    [Required, EmailAddress, MaxLength(100)]
    public string Email { get; set; }
    [Required, MinLength(6), MaxLength(100)]
    public string Contraseña { get; set; }
}

