using API.Data;
using API.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using SharedModels.Models;
using SharedModels.ModelsDTO;
using SharedModels.ModelsDTO.Empresa;

namespace API.Repositories
{
    public class EmpresaRepository : Repository<Empresa>, IEmpresaRepository
    {
        private readonly APIContext _context;

        public EmpresaRepository(APIContext context) : base(context)
        {
            _context = context;
        }


        public async Task<Empresa?> GetByNombreAsync(string nombre)
        {
            return await dbSet.FirstOrDefaultAsync(e => e.Nombre == nombre);
        }

        public async Task<List<Empresa>> GetAllWithUsuariosAsync()
        {
            return await dbSet.Include(e => e.Usuarios).ToListAsync();
        }

        public async Task UpdateAsync(Empresa empresa)
        {
            dbSet.Update(empresa);
            await SaveChangesAsync();
        }
    }
}
