namespace SharedModels.ModelsDTO.Usuario;

public class UsuarioResponseDTO
{
    public int UsuarioId { get; set; }
    public string Nombre { get; set; }
    public string Email { get; set; }
    public string Rol { get; set; }
    public int EmpresaId { get; set; }
    public string EmpresaNombre { get; set; }
}
