using SharedModels.Models;
using SharedModels.ModelsDTO;
using SharedModels.ModelsDTO.Empresa;

namespace API.Repositories.IRepositories;

public interface IEmpresaRepository : IRepository<Empresa>
{
    Task<Empresa?> GetByNombreAsync(string nombre);
    Task<List<Empresa>> GetAllWithUsuariosAsync();
    Task UpdateAsync(Empresa empresa);
}
