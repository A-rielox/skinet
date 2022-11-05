using System.Linq.Expressions;
// el specification evaluator es el q construye el query con esto de aca
// esta en Infrastructure/Data/SpecificationEvaluator.cs
namespace Core.Specifications
{
    public class BaseSpacification<T> : ISpecification<T>
    {
        public BaseSpacification()
        {
        }

        public BaseSpacification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        ////////////////////////////////////////
        ///////////////////////////////////////////
        ///
        public Expression<Func<T, bool>> Criteria { get; }

        public List<Expression<Func<T, object>>> Includes { get; } =
               new List<Expression<Func<T, object>>>();

        ////////////////////////////////////////
        ///////////////////////////////////////////
        ///
        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }
    }
}

// el Criteria puede ser x => x.color === "red"         ( el .Where() )

// en el ProductRepository
//public async Task<IReadOnlyList<Product>> GetProductsAsync()
//{
//    var products = await _context.Products
//        .Include(p => p.ProductType)
//        .Include(p => p.ProductBrand)
//        .ToListAsync();

//    return products;
//}

// la List ... Includes me va a reemplazar los .Include()