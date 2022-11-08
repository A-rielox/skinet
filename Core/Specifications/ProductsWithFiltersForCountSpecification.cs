using Core.Entities;
// crea le query c/el puro .Where() para contar cuantos devuelve
namespace Core.Specifications
{
    public class ProductsWithFiltersForCountSpecification : BaseSpacification<Product>
    {
        public ProductsWithFiltersForCountSpecification(ProductSpecParams productParams)
            : base(x =>
                (string.IsNullOrEmpty(productParams.Search) || x.Name
                                                    .ToLower().Contains(productParams.Search)) &&
                (!productParams.BrandId.HasValue || x.ProductBrandId == productParams.BrandId) &&
                (!productParams.TypeId.HasValue || x.ProductTypeId == productParams.TypeId)
            )
        {
        }
    }
}
