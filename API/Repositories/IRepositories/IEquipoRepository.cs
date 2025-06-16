using SharedModels.Models;

namespace API.Repositories.IRepositories;

public interface IEquipoRepository : IRepository<Equipo>
{
    Task<List<Equipo>> GetByEmpresaIdAsync(int empresaId);
    Task<List<Equipo>> GetByEstadoAsync(string estado);
    Task UpdateAsync(Equipo equipo);
}
