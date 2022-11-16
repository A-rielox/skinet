using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Collections.Generic;
using Core.Specifications;

namespace Infrastructure.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _context;

        public GenericRepository(StoreContext context)
        {
            _context = context;
        }

        ////////////////////////////////////////
        ///////////////////////////////////////////
        ///
        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
        }

        //string? includeProperties = null

        //IQueryable<T> query = dbSet;

        //    if (!tracked)
        //        query = query.AsNoTracking();

        //    if (filter != null)
        //        query = query.Where(filter);

        //    // al necesitar incluir props vendrian como "Villa, VillaSpacial"
        //    if(includeProperties != null)
        //    {
        //        foreach (var includeProp in includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
        //        {
        //            query = query.Include(includeProp);
        //        }
        //    }

        //    return await query.FirstOrDefaultAsync();

        ////////////////////////////////////////
        ///////////////////////////////////////////
        ///
        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        ////////////////////////////////////////
        ///////////////////////////////////////////
        ///
        public async Task<T> GetEntityWithSpec(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        ////////////////////////////////////////
        ///////////////////////////////////////////
        ///
        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }
        ////////////////////////////////////////
        ///////////////////////////////////////////
        ///
        // para solo contar cantidad de elementos YA filtrados
        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        ////////////////////////////////////////
        ///////////////////////////////////////////
        ///
        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
        }

        ////////////////////////////////////////
        ///////////////////////////////////////////
        ///
        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        ////////////////////////////////////////
        ///////////////////////////////////////////
        ///
        public void Update(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        ////////////////////////////////////////
        ///////////////////////////////////////////
        ///
        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
    }
}
