using SharedModels.Models;

namespace API.Repositories.IRepositories;

public interface IUsuarioRepository : IRepository<Usuario>
{
    Task<Usuario?> GetByEmailAsync(string email);
    Task<List<Usuario>> GetByEmpresaIdAsync(int empresaId);
    Task UpdateAsync(Usuario usuario);
}
