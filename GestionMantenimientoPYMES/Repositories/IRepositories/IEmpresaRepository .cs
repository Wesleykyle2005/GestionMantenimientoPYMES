using SharedModels.ModelsDTO;
using SharedModels.ModelsDTO.Empresa;
namespace GestionMantenimientoPYMES.Repositories.IRepositories;

public interface IEmpresaRepository : IRepository<EmpresaResponseDTO>
{
    Task<EmpresaResponseDTO> GetByNombreAsync(string nombre);
    Task<IEnumerable<EmpresaResponseDTO>> GetAllWithUsuariosAsync();
}