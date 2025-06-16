using API.Data;
using API.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace API.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly APIContext _context;
        protected readonly DbSet<T> dbSet;

        public Repository(APIContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            dbSet = _context.Set<T>();
        }

        /// <summary>
        /// Inicia una transacción en la base de datos.
        /// </summary>
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        /// <summary>
        /// Crea una nueva entidad y guarda los cambios.
        /// </summary>
        public async Task CreateAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            await dbSet.AddAsync(entity);
            await SaveChangesAsync();
        }

        /// <summary>
        /// Elimina una entidad y guarda los cambios.
        /// </summary>
        public async Task DeleteAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            dbSet.Remove(entity);
            await SaveChangesAsync();
        }

        /// <summary>
        /// Verifica si existe alguna entidad que cumpla el filtro.
        /// </summary>
        public async Task<bool> ExistsAsync(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.AnyAsync();
        }

        /// <summary>
        /// Obtiene todas las entidades que cumplen el filtro (si se especifica).
        /// </summary>
        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }

        /// <summary>
        /// Obtiene la primera entidad que cumple el filtro (si se especifica).
        /// </summary>
        public async Task<T?> GetAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true)
        {
            IQueryable<T> query = dbSet;
            if (!tracked)
            {
                query = query.AsNoTracking();
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Obtiene una entidad por su clave primaria (asume que la clave es int).
        /// </summary>
        public async Task<T?> GetByIdAsync(int id)
        {
            return await dbSet.FindAsync(id);
        }

        /// <summary>
        /// Guarda todos los cambios pendientes en el contexto.
        /// </summary>
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
