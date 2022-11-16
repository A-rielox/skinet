using Core.Entities;
using Core.Specifications;

namespace Core.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int id);

        Task<IReadOnlyList<T>> ListAllAsync();

        Task<T> GetEntityWithSpec(ISpecification<T> spec);

        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);

        // para solo contar cantidad de elementos YA filtrados
        Task<int> CountAsync(ISpecification<T> spec);

        // solo p'q entityF trackee las entities, la UnitOfWork es la q va a salvar los cambios
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
