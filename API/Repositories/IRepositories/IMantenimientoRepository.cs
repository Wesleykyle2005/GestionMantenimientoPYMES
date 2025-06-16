using SharedModels.Models;

namespace API.Repositories.IRepositories;

public interface IMantenimientoRepository : IRepository<Mantenimiento>
{
    Task<List<Mantenimiento>> GetByEquipoIdAsync(int equipoId);
    Task<List<Mantenimiento>> GetByEstadoAsync(string estado);
    Task<List<Mantenimiento>> GetByEmpresaIdAsync(int empresaId);
    Task<List<Mantenimiento>> GetProximosAsync(DateTime fechaLimite);
    Task UpdateAsync(Mantenimiento mantenimiento);
}
