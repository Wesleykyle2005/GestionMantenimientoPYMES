using API.Data;
using API.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using SharedModels.Models;

namespace API.Repositories
{
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        private readonly APIContext _context;

        public UsuarioRepository(APIContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            return await dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<List<Usuario>> GetByEmpresaIdAsync(int empresaId)
        {
            return await dbSet.Where(u => u.EmpresaId == empresaId).ToListAsync();
        }

        public async Task UpdateAsync(Usuario usuario)
        {
            dbSet.Update(usuario);
            await SaveChangesAsync();
        }
    }
}
