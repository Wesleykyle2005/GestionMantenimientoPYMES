using SharedModels.ModelsDTO;
using SharedModels.ModelsDTO.Usuario;
namespace GestionMantenimientoPYMES.Repositories.IRepositories;

public interface IUsuarioRepository : IRepository<UsuarioResponseDTO>
{
    Task<UsuarioResponseDTO> GetByEmailAsync(string email);
    Task<IEnumerable<UsuarioResponseDTO>> GetByEmpresaIdAsync(int empresaId);
    Task<string> AuthenticateUserAsync(string email, string password);
}