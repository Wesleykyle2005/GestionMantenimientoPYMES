using API.Data;
using API.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using SharedModels.Models;

namespace API.Repositories
{
    public class EquipoRepository : Repository<Equipo>, IEquipoRepository
    {
        private readonly APIContext _context;

        public EquipoRepository(APIContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Equipo>> GetByEmpresaIdAsync(int empresaId)
        {
            return await dbSet.Where(e => e.EmpresaId == empresaId).ToListAsync();
        }

        public async Task<List<Equipo>> GetByEstadoAsync(string estado)
        {
            return await dbSet.Where(e => e.Estado == estado).ToListAsync();
        }

        public async Task UpdateAsync(Equipo equipo)
        {
            dbSet.Update(equipo);
            await SaveChangesAsync();
        }
    }
}
