using Core.Entities;

namespace Core.Specifications
{
    public class ProductsWithTypesAndBrandsSpecification
       : BaseSpecification<Product>
    {
        public ProductsWithTypesAndBrandsSpecification(ProductSpecParams productParams)
            // TODO: Understand better
            : base( p =>
                    (!productParams.BrandId.HasValue || p.ProductBrandId == productParams.BrandId) &&
                    (!productParams.TypeId.HasValue || p.ProductTypeId == productParams.TypeId)
            )
        {
            AddInclude(p => p.ProductType);
            AddInclude(p => p.ProductBrand);

            if (!string.IsNullOrEmpty(productParams.Sort))
            {
                switch (productParams.Sort)
                {
                    case "priceAsc": 
                        AddOrderBy(p => p.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDescending(p => p.Price);
                        break;
                    case "nameAsc":
                        AddOrderBy(p => p.Name);
                        break;
                    case "nameDesc":
                        AddOrderByDescending(p => p.Name);
                        break;
                    default:
                        AddOrderBy(p => p.Id);
                        break;
                }
            }
            else
            {
                AddOrderBy(p => p.Id);
            }

            ApplyPaging(
                // Achieve correct response for page 1 and so on
                productParams.PageSize * (productParams.PageIndex - 1),
                productParams.PageSize
            );
        }
        public ProductsWithTypesAndBrandsSpecification(int id)
            : base(p => p.Id == id)
        {
            AddInclude(p => p.ProductType);
            AddInclude(p => p.ProductBrand);
        }
    }
}