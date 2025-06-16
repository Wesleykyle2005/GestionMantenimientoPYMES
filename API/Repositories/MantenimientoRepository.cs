using API.Data;
using API.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using SharedModels.Models;

namespace API.Repositories
{
    public class MantenimientoRepository : Repository<Mantenimiento>, IMantenimientoRepository
    {
        private readonly APIContext _context;

        public MantenimientoRepository(APIContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Mantenimiento>> GetByEquipoIdAsync(int equipoId)
        {
            return await dbSet.Where(m => m.EquipoId == equipoId).ToListAsync();
        }

        public async Task<List<Mantenimiento>> GetByEstadoAsync(string estado)
        {
            return await dbSet.Where(m => m.Estado == estado).ToListAsync();
        }

        public async Task<List<Mantenimiento>> GetByEmpresaIdAsync(int empresaId)
        {
            return await dbSet
                .Include(m => m.Equipo)
                .Where(m => m.Equipo.EmpresaId == empresaId)
                .ToListAsync();
        }

        public async Task<List<Mantenimiento>> GetProximosAsync(DateTime fechaLimite)
        {
            return await dbSet.Where(m => m.Fecha <= fechaLimite && m.Estado != "Realizado").ToListAsync();
        }

        public async Task UpdateAsync(Mantenimiento mantenimiento)
        {
            dbSet.Update(mantenimiento);
            await SaveChangesAsync();
        }
    }
}
