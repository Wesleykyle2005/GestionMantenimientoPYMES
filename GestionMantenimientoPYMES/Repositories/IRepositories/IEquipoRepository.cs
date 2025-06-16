using SharedModels.ModelsDTO.Equipo;

namespace GestionMantenimientoPYMES.Repositories.IRepositories;

public interface IEquipoRepository : IRepository<EquipoResponseDTO>
{
    Task<IEnumerable<EquipoResponseDTO>> GetByEmpresaIdAsync(int empresaId);
    Task<IEnumerable<EquipoResponseDTO>> GetByEstadoAsync(string estado);
}