using SharedModels.ModelsDTO.Usuario;
namespace SharedModels.ModelsDTO.Empresa;

public class EmpresaResponseDTO
{
    public int EmpresaId { get; set; }
    public string Nombre { get; set; }
    public string Direccion { get; set; }
    public string Telefono { get; set; }
    public List<UsuarioEmpresaDTO> Usuarios { get; set; }
}
