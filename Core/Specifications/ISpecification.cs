﻿using System.Linq.Expressions;

namespace Core.Specifications
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criteria { get; }
        List<Expression<Func<T, object>>> Includes { get; }
    }
}
// el Criteria puede ser x => x.color === "red"         ( el .Where() )


// en el ProductRepository
// public async Task<IReadOnlyList<Product>> GetProductsAsync()
// {
//     var products = await _context.Products
//         .Include(p => p.ProductType)
//         .Include(p => p.ProductBrand)
//         .ToListAsync();
   
//     return products;
// }

// la List ... Includes me va a reemplazar los .Include()