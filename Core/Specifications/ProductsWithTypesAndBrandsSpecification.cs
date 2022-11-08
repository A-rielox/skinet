using Core.Entities;
using System.Linq.Expressions;

namespace Core.Specifications
{
    public class ProductsWithTypesAndBrandsSpecification : BaseSpacification<Product>
    {
        // con || si la expresion es false => hace lo d la derecha
        // si viene alguno => lo pasa al base y ocupa el constructor q pone el Criteria
        // Search se checa con IsNullOrEmpty xq es string, los ...Id son int?
        public ProductsWithTypesAndBrandsSpecification(ProductSpecParams productParams)
            : base( x =>
                (string.IsNullOrEmpty(productParams.Search) || x.Name
                                                    .ToLower().Contains(productParams.Search)) &&
                (!productParams.BrandId.HasValue || x.ProductBrandId == productParams.BrandId) &&
                (!productParams.TypeId.HasValue || x.ProductTypeId == productParams.TypeId)
            )
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);

            AddOrderBy(x => x.Name); // asi x default

            ApplyPaging( productParams.PageSize * (productParams.PageIndex - 1),
                         productParams.PageSize);

            if (!string.IsNullOrEmpty(productParams.Sort))
            {
                switch (productParams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(x => x.Price);
                        break;

                    case "priceDesc":
                        AddOrderByDescending(x => x.Price);
                        break;

                    default:
                        AddOrderBy(n => n.Name);
                        break;
                }
            }
        }

        public ProductsWithTypesAndBrandsSpecification(int id) 
            : base(x => x.Id == id)
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
        }
    }
}
