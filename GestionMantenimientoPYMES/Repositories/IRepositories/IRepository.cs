namespace GestionMantenimientoPYMES.Repositories.IRepositories;

public interface IRepository<T>
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    Task<T> CreateAsync(object dto);
    Task<bool> UpdateAsync(int id, object dto);
    Task<bool> DeleteAsync(int id);
}