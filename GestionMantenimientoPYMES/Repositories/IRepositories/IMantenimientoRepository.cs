using SharedModels.ModelsDTO.Mantenimiento;

namespace GestionMantenimientoPYMES.Repositories.IRepositories;

public interface IMantenimientoRepository : IRepository<MantenimientoResponseDTO>
{
    Task<IEnumerable<MantenimientoResponseDTO>> GetByEquipoIdAsync(int equipoId);
    Task<IEnumerable<MantenimientoResponseDTO>> GetByEstadoAsync(string estado);
    Task<IEnumerable<MantenimientoResponseDTO>> GetByEmpresaIdAsync(int empresaId);
    Task<IEnumerable<MantenimientoResponseDTO>> GetProximosAsync(DateTime fechaLimite);
}