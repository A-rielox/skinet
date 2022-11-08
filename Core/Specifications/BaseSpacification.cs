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

        //------
        public Expression<Func<T, object>> OrderBy { get; private set; }

        public Expression<Func<T, object>> OrderByDescending { get; private set; }

        //------  PAGING
        public int Take { get; private set; }

        public int Skip { get; private set; }

        public bool IsPagingEnabled { get; private set; }

        ////////////////////////////////////////
        ///////////////////////////////////////////
        ///
        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        //------

        protected void AddOrderBy(Expression<Func<T, object>> orderByExpresion)
        {
            OrderBy = orderByExpresion;
        }
        protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpresion)
        {
            OrderByDescending = orderByDescExpresion;
        }

        //------  PAGING
        protected void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
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


//
//
//Use private set when you want setter can't be accessed from outside.

//Use readonly when you want to set the property only once. In the constructor or variable initializer.