using Core.Entities;

namespace Core.Specifications
{
    // Specification for count calculation. Filters matter
    public class ProductWithFiltersForCountSpecification : BaseSpecification<Product>
    {
        public ProductWithFiltersForCountSpecification(ProductSpecParams productParams)
            : base( p =>
                (!productParams.BrandId.HasValue || p.ProductBrandId == productParams.BrandId) &&
                (!productParams.TypeId.HasValue || p.ProductTypeId == productParams.TypeId)
            )
        {
        }
    }
}